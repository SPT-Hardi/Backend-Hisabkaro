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
        public Result Add(object URId, SubmitDaily value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var joindate = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId select x.CreateDate).FirstOrDefault();
                    var OrgStaffAttendanceDailyId = 0;
                    if (joindate.Date <= ISDT.Date)
                    {
                        var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Date == ISDT.Date && x.URId == (int)URId).SingleOrDefault();

                        TimeSpan lateby = new TimeSpan();
                        if (qs == null)
                        {

                            var org = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId).SingleOrDefault();
                            if (org == null)
                            {
                                throw new ArgumentException("Staff not exist in organization!");
                            }
                            if (ISDT.TimeOfDay > org.DevOrganisationsShiftTime.MarkLate)
                            {
                                lateby = ISDT.TimeOfDay - (TimeSpan)org.DevOrganisationsShiftTime.MarkLate;
                            }
                            OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                            attendance.URId = (int)URId;
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
                    }
                    else 
                    {
                        throw new ArgumentException("Not authorized!");
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
        public Result AddFromQr(object URId,Models.Employer.Organization.Staff.Attendance.SubmitDailyThroughQR value) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Date == ISDT.Date && x.URId == (int)URId).SingleOrDefault();
                    var OrgStaffAttendanceDailyId = 0;
                    TimeSpan lateby = new TimeSpan();
                    if (qs == null)
                    {

                        var org = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId).SingleOrDefault();
                        if (org == null)
                        {
                            throw new ArgumentException("Staff not exist in organization!");
                        }
                        if (ISDT.TimeOfDay > org.DevOrganisationsShiftTime.MarkLate)
                        {
                            lateby = ISDT.TimeOfDay - (TimeSpan)org.DevOrganisationsShiftTime.MarkLate;
                        }
                        OrgStaffsAttendancesDaily attendance = new OrgStaffsAttendancesDaily();
                        var qrvalid = c.DevOrganisations.Where(x => x.QRString == value.QRString && x.OId == org.DevOrganisation.OId).SingleOrDefault();
                        if (qrvalid == null) 
                        {
                            throw new ArgumentException("QR Invalid!");
                        }
                        var fileid = c.CommonFiles.Where(x => x.FGUID == value.FGUID).SingleOrDefault();
                        if (fileid == null) 
                        {
                            throw new ArgumentException("File not exist!");
                        }
                        attendance.URId = (int)URId;
                        attendance.LastUpdateDate = ISDT;
                        attendance.ChekIN = ISDT;
                        attendance.PhotoFileId = fileid.FileId;
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
                        throw new ArgumentException("You only checkin through qr!");
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
        public Result Get(object URId) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var staffattendance = c.OrgStaffsAttendancesDailies.Where(x => x.URId == (int)URId && x.ChekIN.Value.Date == ISDT.Date).SingleOrDefault();
                    bool IsPresent = false;
                    DateTime? LastUpdate =null;
                    if (staffattendance != null) 
                    {
                        IsPresent = true;
                        LastUpdate = staffattendance.CheckOUT == null ? staffattendance.ChekIN : staffattendance.CheckOUT;
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
