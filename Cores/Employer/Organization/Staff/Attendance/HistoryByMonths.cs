using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroDBContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class HistoryByMonths
    {
        public Result Get(int URId,DateTime date)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    List<AttendanceHistory> attendanceHistory = new List<AttendanceHistory>();
                    var lateby = new TimeSpan();
                    var presentcount = 0;
                    var latecount = 0;
                    var currentmonth = date.Month;
                    var currentyear = date.Year;
                    var totalworkinghourmonth = new TimeSpan();
                    var days = getTotalDays(date.Date);
                    var Org = c.DevOrganisationsStaffs.Where(x => x.URId == URId).SingleOrDefault();
                    for (var i = 1; i <= days; i++)
                    {
                        var checkindate = DateTime.Parse($"{currentyear}-{currentmonth}-{i}");
                        var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.URId == URId && x.ChekIN.Value.Date == checkindate).SingleOrDefault();
                        if (checkpresent != null)
                        {
                            presentcount += 1;

                            var lateafter = (from obj in c.DevOrganisationsStaffs
                                             join obj1 in c.DevOrganisationsShiftTimes
                                             on obj.ShiftTimeId equals obj1.ShiftTimeId
                                             where obj.URId == URId
                                             select obj1).SingleOrDefault();
                            if (lateafter == null)
                            {
                                throw new ArgumentException("Register user as staff,make shiftId entry!");
                            }
                            if (checkpresent.ChekIN.Value.TimeOfDay > lateafter.MarkLate)
                            {
                                latecount += 1;
                                lateby = checkpresent.ChekIN.Value.TimeOfDay - (TimeSpan)lateafter.MarkLate;
                            }
                            var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                            var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                            attendanceHistory.Add(new AttendanceHistory()
                            {
                                URId =URId,
                                AttendanceDate =$"{i} {monthname} | {dayname}",
                                Status ="Present",
                                CheckIN = checkpresent.ChekIN.Value.TimeOfDay.ToString(@"hh\:mm"),
                                CheckOUT = checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                LateBy = lateby.ToString(@"hh\:mm"),
                                TotalWorkingHourPerDay =(checkpresent.CheckOUT.Value.TimeOfDay-checkpresent.ChekIN.Value.TimeOfDay).ToString(@"hh\:mm"),

                            });
                            totalworkinghourmonth += (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.Value.TimeOfDay);
                        }
                        else 
                        {
                            var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                            var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                            attendanceHistory.Add(new AttendanceHistory()
                            {
                                URId = URId,
                                AttendanceDate = $"{i} {monthname} | {dayname}",
                                Status = "Absent",
                                CheckIN = "00:00",
                                CheckOUT = "00:00",
                                LateBy = "00:00",
                                TotalWorkingHourPerDay = "00:00",

                            });
                        }
                      
                    }
                    var totalhour = Math.Floor((totalworkinghourmonth.TotalMinutes) / 60);
                    var remainminute = (totalworkinghourmonth.TotalMinutes) - (totalhour*60);
                    Models.Employer.Organization.Staff.Attendance.HistoryByMonth historyByMonth = new HistoryByMonth();
                    historyByMonth.attendanceHistory = attendanceHistory;
                    historyByMonth.AttendanceMonth = $"{date.ToString("MMMM").Substring(0, 3)},{date.Year}";
                    historyByMonth.TotalWorkingHourPerMonth = $"{totalhour}:{remainminute}";
                    historyByMonth.URId = URId;
                    historyByMonth.Name = Org.SubUserOrganisation.SubUser.SubUsersDetail.FullName;
                    historyByMonth.ImagePath = Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath ;
                    historyByMonth.MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber;
                    historyByMonth.Present = presentcount;
                    historyByMonth.Absent = days - presentcount;
                    historyByMonth.Late = latecount;
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance-history by month get successfully!",
                        Data = historyByMonth,
                    };
                }
            }

        }
        public int getTotalDays(DateTime month) 
        {
            if (month.Month == 1 || month.Month == 3 || month.Month == 5 || month.Month == 7 || month.Month == 8 || month.Month == 10 || month.Month == 12)
            {
                return 31;
            }
            else if (month.Month == 2 && ((month.Year) % 4) == 0) 
            {
                return 29;
            }
            else if(month.Month == 2 && ((month.Year) % 4) != 0)
            {
                return 28;
            }
            else
            {
                return 30;
            }
        }
       
    }
}
