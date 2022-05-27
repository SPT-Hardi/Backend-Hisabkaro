using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class HistoryByDays
    {
        public Result Get(object URId, DateTime date)
        {
            using (DBContext c = new DBContext())
            {
                AttendanceHistory attendanceHistory = new AttendanceHistory();
                var userorg = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();
                var joindate = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId select x.CreateDate).FirstOrDefault();
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);


                var Org = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId && x.DevOrganisation.OId == userorg.OId).SingleOrDefault();
                if (Org == null)
                {
                    throw new ArgumentException("Staff not exist in organization!");
                }
                if (joindate > date)
                {
                    throw new ArgumentException("User yet not join your organzation!,JoinDate > Given Date");
                }
                var checkindate = date;
                var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.ChekIN.Value.Date == checkindate.Date && x.URId == (int)URId).SingleOrDefault();
                if (checkpresent != null)
                {
                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                    var TotalWorkingHourPerDay = checkpresent.CheckOUT == null ? ((checkindate < ISDT) ? (checkpresent.ShiftEndTime - checkpresent.ChekIN.Value.TimeOfDay) : null) : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.Value.TimeOfDay);

                    var res = new
                    {

                        URId = (int)URId,
                        AttendanceDate = $"{date.Day} {monthname} | {dayname}",
                        Date = checkindate,
                        Status = checkpresent.IsOvertime == true ? "OverTime" : "Present",
                        CheckIN = checkpresent.ChekIN.Value.TimeOfDay.ToString(@"hh\:mm"),
                        CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                        LateBy = checkpresent.Lateby == null ? null : checkpresent.Lateby.Value.ToString(@"hh\:mm"),
                        TotalWorkingHourPerDay = TotalWorkingHourPerDay.Value.ToString(@"hh\:mm"),
                        Name = Org.NickName,
                        ImagePath = checkpresent.PhotoFileId == null ? (Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID) : checkpresent.CommonFile.FGUID,
                        MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber
                };
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "HistoryBy Days geted successfully!",
                        Data = res,
                    };
                }
                else 
                {
                    var WeekoffDay = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId && (x.WeekOffOneDay == null ? false : x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower() || x.WeekOffSecondDay == null ? false : x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower()) select x).FirstOrDefault();

                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                    var res = new
                    {
                        URId = (int)URId,
                        AttendanceDate = $"{date.Day} {monthname} | {dayname}",
                        Date = checkindate,
                        Status = (WeekoffDay == null) ? "Absent" : "WeekOff",
                        CheckIN = "00:00",
                        CheckOUT = "00:00",
                        LateBy = "00:00",
                        TotalWorkingHourPerDay = "00:00",
                        Name = Org.NickName,
                        ImagePath = Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID,
                        MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber

                    };
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "HistoryBy Days geted successfully!",
                        Data = res,
                    };

                }
                   
            }
        }
    }
}
