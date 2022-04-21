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
        public Result Add(int URId,SubmitDaily value)
        {
            
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Date == DateTime.Now.Date && x.URId == URId).SingleOrDefault();
                    var OrgStaffAttendanceDailyId = 0;
                    bool late=false;
                    if (qs == null)
                     {

                        var org = c.DevOrganisationsStaffs.Where(x => x.URId == URId).SingleOrDefault();
                        if (org == null) 
                        {
                            throw new ArgumentException("Staff not exist in organization!");
                        }
                        if (DateTime.Now.TimeOfDay > org.DevOrganisationsShiftTime.MarkLate) 
                        {
                            late = true;
                        }
                        OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                        attendance.URId = URId;
                        attendance.LastUpdateDate = DateTime.Now;
                        attendance.ChekIN = DateTime.Now;
                        attendance.IsLate = late;
                        c.OrgStaffsAttendancesDailies.InsertOnSubmit(attendance);
                        c.SubmitChanges();
                        OrgStaffAttendanceDailyId = attendance.OrgStaffAttendanceDailyId;
                    
                     }
                    else
                    {
                        qs.CheckOUT = DateTime.Now;
                        qs.LastUpdateDate = DateTime.Now;
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
                            LastUpdate = DateTime.Now,
                        },
                    };
                }
            }
        }
    }
}
