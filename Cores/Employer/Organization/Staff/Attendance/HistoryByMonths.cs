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
        public Result Get(int URId,int Id,DateTime date)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    List<AttendanceHistory> attendanceHistory = new List<AttendanceHistory>();
                    //var lateby = new TimeSpan();
                    var presentcount = 0;
                    var latecount = 0;
                    var currentdate = DateTime.Now.ToLocalTime();
                    var requestmonth = date.Month;
                    var requestyear = date.Year;
                    var totalworkinghourmonth = new TimeSpan();
                    //var days = getTotalDays(date.Date);
                    HistoryByMonth historyByMonth = new HistoryByMonth();
                    var userorg = c.SubUserOrganisations.Where(x => x.URId == URId).SingleOrDefault();

                    URId = Id == 0 ? URId : Id;
                    var Org = c.DevOrganisationsStaffs.Where(x => x.URId == URId && x.DevOrganisation.OId==userorg.OId).SingleOrDefault();
                    if (Org == null) 
                    {
                        throw new ArgumentException("Staff not exist in organization!");
                    }
                    //for past month 
                    if (date.Month < DateTime.Now.ToLocalTime().Month && date.Year <= DateTime.Now.ToLocalTime().Year)
                    {

                        var checkpresentlist = c.OrgStaffsAttendancesDailies.Where(x => x.URId == URId && x.ChekIN.Value.Month == requestmonth && x.ChekIN.Value.Year == requestyear).ToList();
                        int days = DateTime.DaysInMonth(date.Year, date.Month);
                        for (var i = 1; i <= days; i++)
                        {
                            var checkindate = DateTime.Parse($"{requestyear}-{requestmonth}-{i}");
                            var checkpresent = checkpresentlist.Where(x => x.ChekIN.Value.Date == checkindate.Date).SingleOrDefault();
                            if (checkpresent != null)
                            {
                                presentcount += 1;

                                
                                if (checkpresent.Lateby!=null)
                                {
                                    latecount += 1;
                                    
                                }
                                var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                attendanceHistory.Add(new AttendanceHistory()
                                {
                                    URId = URId,
                                    AttendanceDate = $"{i} {monthname} | {dayname}",
                                    Date = checkindate,
                                    Status = "Present",
                                    CheckIN = checkpresent.ChekIN.Value.TimeOfDay.ToString(@"hh\:mm"),
                                    CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                    LateBy = checkpresent.Lateby==null ? null  :checkpresent.Lateby.Value.ToString(@"hh\:mm"),
                                    TotalWorkingHourPerDay = checkpresent.CheckOUT == null ? null : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.Value.TimeOfDay).ToString(@"hh\:mm"),

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
                                    Date = checkindate,
                                    Status = "Absent",
                                    CheckIN = "00:00",
                                    CheckOUT = "00:00",
                                    LateBy = "00:00",
                                    TotalWorkingHourPerDay = "00:00",

                                });
                            }

                        }
                        historyByMonth.Present = presentcount;
                        historyByMonth.Absent = days - presentcount;
                        historyByMonth.Late = latecount;
                    }
                    //for current month
                    else
                    {
                        DateTime startDate = new DateTime(date.Year, date.Month, day: 1);
                        int days = (int)(((DateTime.Now.ToLocalTime() - startDate).TotalDays)+1);
                        var checkpresentlist = c.OrgStaffsAttendancesDailies.Where(x => x.URId == URId && x.ChekIN.Value.Month == requestmonth && x.ChekIN.Value.Year == requestyear).ToList();
                        for (var i = 1; i <= days; i++)
                        {
                            var checkindate = DateTime.Parse($"{requestyear}-{requestmonth}-{i}");
                            var checkpresent = checkpresentlist.Where(x => x.ChekIN.Value.Date == checkindate.Date).SingleOrDefault();
                            if (checkpresent != null)
                            {
                                presentcount += 1;

                                if (checkpresent.Lateby !=null)
                                {
                                    latecount += 1;
                                    
                                }
                                var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                var TotalWorkingHourPerDay = checkpresent.CheckOUT == null ? new TimeSpan() : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.Value.TimeOfDay);
                                attendanceHistory.Add(new AttendanceHistory()
                                {
                                    URId = URId,
                                    AttendanceDate = $"{i} {monthname} | {dayname}",
                                    Date = checkindate,
                                    Status = "Present",
                                    CheckIN = checkpresent.ChekIN.Value.TimeOfDay.ToString(@"hh\:mm"),
                                    CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                    LateBy = checkpresent.Lateby == null ? null : checkpresent.Lateby.Value.ToString(@"hh\:mm"),
                                    TotalWorkingHourPerDay = checkpresent.CheckOUT==null? null : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.Value.TimeOfDay).ToString(@"hh\:mm"),

                                });
                                //@"hh\:mm"
                                totalworkinghourmonth += TotalWorkingHourPerDay;
                            }
                            else
                            {
                                var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                attendanceHistory.Add(new AttendanceHistory()
                                {
                                    URId = URId,
                                    AttendanceDate = $"{i} {monthname} | {dayname}",
                                    Date = checkindate,
                                    Status = "Absent",
                                    CheckIN = "00:00",
                                    CheckOUT = "00:00",
                                    LateBy = "00:00",
                                    TotalWorkingHourPerDay = "00:00",

                                });
                            }

                            historyByMonth.Present = presentcount;
                            historyByMonth.Absent = days - presentcount;
                            historyByMonth.Late = latecount;
                        }
                        

                    }
                    var totalhour = (Math.Floor((totalworkinghourmonth.TotalMinutes) / 60));
                    var remainminute = Math.Floor((totalworkinghourmonth.TotalMinutes) - (totalhour * 60));

                    historyByMonth.attendanceHistory = attendanceHistory;
                    historyByMonth.AttendanceMonth = $"{date.ToString("MMMM").Substring(0, 3)},{date.Year}";
                    historyByMonth.TotalWorkingHourPerMonth = ($"{totalhour}:{remainminute}");
                    historyByMonth.URId = URId;
                    historyByMonth.Name = Org.SubUserOrganisation.SubUser.SubUsersDetail.FullName;
                    historyByMonth.ImagePath = Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath;
                    historyByMonth.MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber;
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
       /* public TimeSpan getCheckout(DateTime checkout)
        {
            if (checkout == null)
            {
                return 0;
            }
            else 
            {
                return checkout.TimeOfDay;
            }
        }*/

    }
}
