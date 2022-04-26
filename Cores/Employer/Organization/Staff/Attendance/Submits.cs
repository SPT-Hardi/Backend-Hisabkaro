using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Submits
    {
        public Result Add(int URId, SubmitDaily value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Date == ISDT.Date && x.URId == URId).SingleOrDefault();
                    var OrgStaffAttendanceDailyId = 0;
                    TimeSpan lateby = new TimeSpan();
                    if (qs == null)
                    {

                        var org = c.DevOrganisationsStaffs.Where(x => x.URId == URId).SingleOrDefault();
                        if (org == null)
                        {
                            throw new ArgumentException("Staff not exist in organization!");
                        }
                        if (ISDT.TimeOfDay > org.DevOrganisationsShiftTime.MarkLate)
                        {
                            lateby = ISDT.TimeOfDay - (TimeSpan)org.DevOrganisationsShiftTime.MarkLate;
                        }
                        OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                        attendance.URId = URId;
                        attendance.LastUpdateDate = ISDT;
                        attendance.ChekIN = ISDT;
                        attendance.Lateby = lateby;
                        c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                        c.SubmitChanges();
                        OrgStaffAttendanceDailyId = attendance.OrgStaffAttendanceDailyId;

                    }
                    else
                    {
                        if (qs.IsAccessible == false)
                        {
                            throw new ArgumentException("Permission revoked!");
                        }
                        qs.CheckOUT = ISDT;
                        qs.LastUpdateDate = ISDT;
                        c.SubmitChanges();
                        OrgStaffAttendanceDailyId = qs.OrgStaffAttendanceDailyId;
                    }

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff Daily Attendance Add Successfully!",
                        Data = new
                        {
                            OrgStaffsAttendancesDailyId = OrgStaffAttendanceDailyId,
                            LastUpdate = ISDT.ToString("hh:mm tt"),
                        },
                    };
                }
            }
        }
        public Result Get(int URId) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var staffattendance = c.OrgStaffsAttendancesDailies.Where(x => x.URId == URId && x.ChekIN.Value.Date == ISDT.Date).SingleOrDefault();
                    bool IsPresent = false;
                    string LastUpdate = null;
                    if (staffattendance != null) 
                    {
                        IsPresent = true;
                        LastUpdate = staffattendance.CheckOUT == null ? Convert.ToDateTime(staffattendance.ChekIN.ToString()).ToString("hh:mm tt") : Convert.ToDateTime(staffattendance.CheckOUT.ToString()).ToString("hh:mm tt");
                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff Daily Attendance Add Successfully!",
                        Data = new
                        {
                            URId = URId,
                            TodayDate =ISDT.ToString("dd/MM/yyyy"),
                            IsPresent =IsPresent,
                            LastUpdate = LastUpdate,
                        },
                    };
                }
            }

        }
    }
}
