using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class HistoryByMonths
    {
        /*public Result Get(object URId, int Id, DateTime date)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                HistoryByMonth historyByMonth = new HistoryByMonth();
                using (DBContext c = new DBContext())
                {
                    List<AttendanceHistory> attendanceHistory = new List<AttendanceHistory>();
                    //var lateby = new TimeSpan();
                    var presentcount = 0;
                    var overtimecount = 0;
                    var latecount = 0;
                    var weekoffcount = 0;
                    var currentdate = ISDT;
                    var requestmonth = date.Month;
                    var requestyear = date.Year;
                    var totalworkinghourmonth = new TimeSpan();
                    //var days = getTotalDays(date.Date);
                    var userorg = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();
                    URId = Id == 0 ? URId : Id;
                    var joindate = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId select x.CreateDate).FirstOrDefault();

                    var Org = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId && x.DevOrganisation.OId == userorg.OId).SingleOrDefault();
                    if (Org == null)
                    {
                        throw new ArgumentException("Staff not exist in organization!");
                    }
                    if (joindate.Month <= date.Month && joindate.Year <= date.Year)
                    {

                        //for past month 
                        if (date.Month < ISDT.Month && date.Year <= ISDT.Year)
                        {
                            var startDate = new DateTime();
                            int workingdays = 0;
                            if (joindate.Month == date.Month && joindate.Year == date.Year)
                            {
                                workingdays = (DateTime.DaysInMonth(date.Year, date.Month) - joindate.Day) + 1;
                                startDate = new DateTime(date.Year, date.Month, day: joindate.Day);
                            }
                            else
                            {
                                startDate = new DateTime(date.Year, date.Month, day: 1);
                                workingdays = DateTime.DaysInMonth(date.Year, date.Month);
                            }
                            var checkpresentlist = c.OrgStaffsAttendancesDailies.Where(x => x.URId == (int)URId && x.ChekIN.Month == requestmonth && x.ChekIN.Year == requestyear).ToList();
                            var days = DateTime.DaysInMonth(date.Year, date.Month);

                            for (var i = startDate.Day; i <= days; i++)
                            {
                                var checkindate = DateTime.Parse($"{requestyear}-{requestmonth}-{i}");
                                var checkpresent = checkpresentlist.Where(x => x.ChekIN.Date == checkindate.Date).SingleOrDefault();
                                if (checkpresent != null)
                                {
                                    presentcount += 1;
                                    if (checkpresent.IsOvertimeFullDay == true || checkpresent.IsOvertimeHalfDay == true)
                                    {
                                        overtimecount += 1;
                                    }


                                    if (checkpresent.Lateby != new TimeSpan())
                                    {
                                        latecount += 1;

                                    }
                                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                    var TotalWorkingHourPerDay = checkpresent.CheckOUT == null ? ((checkindate < ISDT) ? (checkpresent.ShiftEndTime - checkpresent.ChekIN.TimeOfDay) : null) : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.TimeOfDay);

                                    attendanceHistory.Add(new AttendanceHistory()
                                    {
                                        URId = (int)URId,
                                        AttendanceDate = $"{i} {monthname} | {dayname}",
                                        Date = checkindate,
                                        Status = (checkpresent.IsOvertimeFullDay == true || checkpresent.IsOvertimeHalfDay == true) ? "OverTime" : "Present",
                                        CheckIN = checkpresent.ChekIN.TimeOfDay.ToString(@"hh\:mm"),
                                        CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                        LateBy = checkpresent.Lateby == new TimeSpan() ? null : checkpresent.Lateby.ToString(@"hh\:mm"),
                                        TotalWorkingHourPerDay = TotalWorkingHourPerDay.Value.ToString(@"hh\:mm")
                                    });
                                    totalworkinghourmonth += (TimeSpan)TotalWorkingHourPerDay;
                                }
                                else
                                {
                                    var WeekoffDay = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId && (x.WeekOffOneDay == null ? false : x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower() || x.WeekOffSecondDay == null ? false : x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower()) select x).FirstOrDefault();
                                    if (WeekoffDay != null)
                                    {
                                        weekoffcount += 1;
                                    }
                                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                    attendanceHistory.Add(new AttendanceHistory()
                                    {
                                        URId = (int)URId,
                                        AttendanceDate = $"{i} {monthname} | {dayname}",
                                        Date = checkindate,
                                        Status = (WeekoffDay == null) ? "Absent" : "WeekOff",
                                        CheckIN = "00:00",
                                        CheckOUT = "00:00",
                                        LateBy = "00:00",
                                        TotalWorkingHourPerDay = "00:00",

                                    });
                                }

                            }
                            historyByMonth.Present = presentcount;
                            historyByMonth.Absent = (workingdays - presentcount) - weekoffcount;
                            historyByMonth.Late = latecount;
                            historyByMonth.WeeklyOff = weekoffcount;
                            historyByMonth.OverTime = overtimecount;
                        }

                        //for current month
                        else
                        {
                            var startDate = new DateTime();
                            if (joindate.Month == date.Month && joindate.Year == date.Year)
                            {

                                startDate = new DateTime(date.Year, date.Month, day: joindate.Day);
                            }
                            else
                            {
                                startDate = new DateTime(date.Year, date.Month, day: 1);
                            }
                            int days = (int)(((ISDT - startDate).TotalDays) + 1);
                            var checkpresentlist = c.OrgStaffsAttendancesDailies.Where(x => x.URId == (int)URId && x.ChekIN.Month == requestmonth && x.ChekIN.Year == requestyear).ToList();
                            for (var i = startDate.Day; i <= ISDT.Day; i++)
                            {
                                var checkindate = DateTime.Parse($"{requestyear}-{requestmonth}-{i}");
                                var checkpresent = checkpresentlist.Where(x => x.ChekIN.Date == checkindate.Date).SingleOrDefault();
                                if (checkpresent != null)
                                {
                                    presentcount += 1;
                                    if (checkpresent.IsOvertimeFullDay == true || checkpresent.IsOvertimeHalfDay == true)
                                    {
                                        overtimecount += 1;
                                    }
                                    if (checkpresent.Lateby != new TimeSpan())
                                    {
                                        latecount += 1;

                                    }
                                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                    var TotalWorkingHourPerDay = checkpresent.CheckOUT == null ? ((checkindate < ISDT) ? (checkpresent.ShiftEndTime - checkpresent.ChekIN.TimeOfDay) : null) : (checkpresent.CheckOUT.Value.TimeOfDay - checkpresent.ChekIN.TimeOfDay);
                                    attendanceHistory.Add(new AttendanceHistory()
                                    {
                                        URId = (int)URId,
                                        AttendanceDate = $"{i} {monthname} | {dayname}",
                                        Date = checkindate,
                                        Status = (checkpresent.IsOvertimeFullDay == true || checkpresent.IsOvertimeHalfDay == true) ? "OverTime" : "Present",
                                        CheckIN = checkpresent.ChekIN.TimeOfDay.ToString(@"hh\:mm"),
                                        CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                        LateBy = checkpresent.Lateby == new TimeSpan() ? null : checkpresent.Lateby.ToString(@"hh\:mm"),
                                        TotalWorkingHourPerDay = TotalWorkingHourPerDay.Value.ToString(@"hh\:mm")
                                    });
                                    //@"hh\:mm"
                                    totalworkinghourmonth += (TimeSpan)TotalWorkingHourPerDay;
                                }
                                else
                                {
                                    var WeekoffDay = (from x in c.DevOrganisationsStaffs where (x.URId == (int)URId) && ((x.WeekOffOneDay == null ? false : x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower()) || (x.WeekOffSecondDay == null ? false : x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == checkindate.DayOfWeek.ToString().ToLower())) select x).FirstOrDefault();
                                    if (WeekoffDay != null)
                                    {
                                        weekoffcount += 1;
                                    }
                                    var dayname = checkindate.DayOfWeek.ToString().Substring(0, 3);
                                    var monthname = checkindate.ToString("MMMM").Substring(0, 3);
                                    attendanceHistory.Add(new AttendanceHistory()
                                    {
                                        URId = (int)URId,
                                        AttendanceDate = $"{i} {monthname} | {dayname}",
                                        Date = checkindate,
                                        Status = (WeekoffDay == null) ? "Absent" : "WeekOff",
                                        CheckIN = "00:00",
                                        CheckOUT = "00:00",
                                        LateBy = "00:00",
                                        TotalWorkingHourPerDay = "00:00",

                                    });
                                }

                                historyByMonth.Present = presentcount;
                                historyByMonth.Absent = (days - presentcount) - weekoffcount;
                                historyByMonth.Late = latecount;
                                historyByMonth.WeeklyOff = weekoffcount;
                                historyByMonth.OverTime = overtimecount;
                            }



                        }
                    }
                    var totalhour = (Math.Floor((totalworkinghourmonth.TotalMinutes) / 60));
                    var remainminute = Math.Floor((totalworkinghourmonth.TotalMinutes) - (totalhour * 60));

                    historyByMonth.attendanceHistory = attendanceHistory;
                    historyByMonth.AttendanceMonth = $"{date.ToString("MMMM").Substring(0, 3)},{date.Year}";
                    historyByMonth.TotalWorkingHourPerMonth = ($"{totalhour}:{remainminute}");
                    historyByMonth.URId = (int)URId;
                    historyByMonth.Name = Org.NickName;
                    historyByMonth.ImagePath = Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID;
                    historyByMonth.MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber;



                }
                scope.Complete();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Staff attendance-history by month get successfully!",
                    Data = historyByMonth,
                };
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
            else if (month.Month == 2 && ((month.Year) % 4) != 0)
            {
                return 28;
            }
            else
            {
                return 30;
            }
        }

        public Result StatisticsByMonth(object URId, DateTime date)
        {
            using (DBContext c = new DBContext())
            {
                if (URId == null)
                {
                    throw new ArgumentException("Token not-valid or expired!");
                }
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                HistoryByMonth historyByMonth = new HistoryByMonth();
                var weekoffcount = 0;
                var currentdate = ISDT;
                var requestmonth = date.Month;
                var requestyear = date.Year;

                //var days = getTotalDays(date.Date);
                var userorg = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();
                var joindate = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId select x.CreateDate).FirstOrDefault();

                var Org = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId && x.DevOrganisation.OId == userorg.OId).SingleOrDefault();
                if (Org == null)
                {
                    throw new ArgumentException("Staff not exist in organization!");
                }
                if (joindate.Month <= date.Month && joindate.Year <= date.Year)
                {
                    var startDate = new DateTime();
                    var daysinmonth = 0;
                    var workingdays = 0;
                    var totalworkinghour = new TimeSpan();
                    var endDate = new DateTime();
                    //for past month 
                    if (date.Month < ISDT.Month && date.Year <= ISDT.Year)
                    {
                        if (joindate.Month == date.Month && joindate.Year == date.Year)
                        {
                            startDate = new DateTime(date.Year, date.Month, day: joindate.Day);
                            daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);
                            workingdays = (DateTime.DaysInMonth(date.Year, date.Month) - joindate.Day) + 1;
                            endDate = new DateTime(date.Year, date.Month, day: daysinmonth);
                        }
                        else
                        {
                            startDate = new DateTime(date.Year, date.Month, day: 1);
                            daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);
                            workingdays = daysinmonth;
                            endDate = new DateTime(date.Year, date.Month, day: daysinmonth);
                        }
                    }
                    else
                    {
                        if (joindate.Month == date.Month && joindate.Year == date.Year)
                        {
                            startDate = new DateTime(date.Year, date.Month, day: joindate.Day);
                            daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);
                            workingdays = (int)(((ISDT - startDate).TotalDays) + 1);
                            endDate = ISDT;
                        }
                        else
                        {
                            startDate = new DateTime(date.Year, date.Month, day: 1);
                            daysinmonth = DateTime.DaysInMonth(date.Year, date.Month);
                            workingdays = (int)(((ISDT - startDate).TotalDays) + 1);
                            endDate = ISDT;
                        }
                    }
                    var present = (from x in c.OrgStaffsAttendancesDailies where x.ChekIN.Date >= startDate.Date && x.ChekIN.Date <= endDate.Date && x.URId == (int)URId select x).ToList();
                    foreach (var x in present)
                    {
                        totalworkinghour += (TimeSpan)(x.CheckOUT == null ? ((x.ChekIN.Date < ISDT.Date) ? (x.ShiftEndTime - x.ChekIN.TimeOfDay) : null) : (x.CheckOUT.Value.TimeOfDay - x.ChekIN.TimeOfDay));
                    }
                    var late = (from x in present
                                where x.Lateby != new TimeSpan()
                                select x).ToList();
                    var overtime = (from x in present
                                    where x.IsOvertimeFullDay == true || x.IsOvertimeHalfDay==true
                                    select x).ToList();
                    var totalhour = (Math.Floor((totalworkinghour.TotalMinutes) / 60));
                    var remainminute = Math.Floor((totalworkinghour.TotalMinutes) - (totalhour * 60));
                    for (var i = startDate.Day; i <= endDate.Day; i++)
                    {
                        var todaydate = DateTime.Parse($"{requestyear}-{requestmonth}-{i}");
                        var WeekOff = (from x in c.DevOrganisationsStaffs
                                       where
                                       x.URId == (int)URId &&
                                       ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                       select x).FirstOrDefault();
                        if (WeekOff != null)
                        {
                            weekoffcount += 1;
                        }
                    }
                    historyByMonth.Present = present.Count();
                    historyByMonth.Absent = (workingdays - present.Count()) - weekoffcount;
                    historyByMonth.Late = late.Count();
                    historyByMonth.WeeklyOff = weekoffcount;
                    historyByMonth.OverTime = overtime.Count();

                    historyByMonth.AttendanceMonth = $"{date.ToString("MMMM").Substring(0, 3)},{date.Year}";
                    historyByMonth.TotalWorkingHourPerMonth = ($"{totalhour}:{remainminute}");
                    historyByMonth.URId = (int)URId;
                    historyByMonth.Name = Org.NickName;
                    historyByMonth.ImagePath = Org.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : Org.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID;
                    historyByMonth.MobileNumber = Org.SubUserOrganisation.SubUser.MobileNumber;
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "StatisticsByMonth of user geted successfully!",
                    Data = historyByMonth,
                };
            }
        }
        public Result StatisticsByDateRange(int URId, DateTime startDate, DateTime endDate)
        {
            using (DBContext c = new DBContext()) 
            {
                StatisticsByDateRange statisticsByDateRange = new StatisticsByDateRange();
                List<Status> statuslist = new List<Status>();
                var paidlist = new Statistics().PaidLeaveList(URId,startDate);
                var totalpresent = 0;
                var totalabsent = 0;
                var totalweeklyoff = 0;
                var totalfullovertime = 0;
                var totalhalfovertime = 0;
                var totalpaidleave = 0;
                var totallate = 0;
                var totaldays = 0;
                var joindate = (from x in c.DevOrganisationsStaffs where x.URId == (int)URId select x.CreateDate).FirstOrDefault();
                if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
                {
                    for (var i = startDate.Day; i <= endDate.Day; i++)
                    {
                        var todaydate = DateTime.Parse($"{startDate.Year}-{startDate.Month}-{i}");
                        if (joindate.Date <= todaydate.Date)
                        {
                            totaldays += 1;
                            var isweekoff = false;
                            var ispresent = false;
                            var isovertimefull = false;
                            var isovertimehalf = false;
                            var ispaidleave = false;
                            var islate = false;
                            var present = (from x in c.OrgStaffsAttendancesDailies where x.ChekIN.Date == todaydate.Date && x.URId == (int)URId select x).FirstOrDefault();
                            if (present != null)
                            {
                                ispresent = true;
                                totalpresent += 1;
                                islate = present.Lateby != new TimeSpan() ? true : false;
                                if (islate)
                                {
                                    totallate += 1;
                                }
                                isovertimefull = present.IsOvertimeFullDay;
                                if (isovertimefull)
                                {
                                    totalfullovertime += 1;
                                }
                                isovertimehalf = present.IsOvertimeHalfDay;
                                if (isovertimehalf)
                                {
                                    totalhalfovertime += 1;
                                }
                            }
                            else
                            {
                                totalabsent += 1;
                            }
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (paidlist != null)
                            {
                                if ((from x in paidlist where x.Date == todaydate.Date select x).Any() ? true : false && !ispresent)
                                {
                                    ispaidleave = true;
                                    totalpaidleave += 1;
                                }
                            }
                            if (WeekOff != null)
                            {
                                isweekoff = true;
                                totalweeklyoff += 1;
                            }
                            *//*
                             Present-P
                             Absent-A
                             WeeklyOff-WO
                             Overtime-OT
                             PaidLeave-PL
                             Late/HalfDay-L
                             *//*
                            statuslist.Add(new Status()
                            {
                                Date = todaydate,
                                IsAbsent = ispresent != true,
                                IsLate = islate,
                                IsOvertimeFull=isovertimefull,
                                IsOvertimeHalf=isovertimehalf,
                                IsPaidLeave = ispaidleave,
                                IsPresent = ispresent,
                                IsWeeklyOff = isweekoff,
                                StatusString=BuilStatusString(ispresent,!ispresent,isovertimefull,isovertimehalf,isweekoff,islate,ispaidleave)
                            });
                        }
                        else 
                        {
                        }
                    }
                }
                else 
                {
                    var daysinmonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                    for (var i = startDate.Day; i <= daysinmonth; i++) 
                    {
                        var todaydate = DateTime.Parse($"{startDate.Year}-{startDate.Month}-{i}");
                        if (joindate.Date <= todaydate.Date)
                        {
                            totaldays += 1;
                            var isweekoff = false;
                            var ispresent = false;
                            var isovertimefull = false;
                            var isovertimehalf = false;
                            var ispaidleave = false;
                            var islate = false;
                            var present = (from x in c.OrgStaffsAttendancesDailies where x.ChekIN.Date == todaydate.Date && x.URId == (int)URId select x).FirstOrDefault();
                            if (present != null)
                            {
                                ispresent = true;
                                totalpresent += 1;
                                islate = present.Lateby != new TimeSpan() ? true : false;
                                if (islate)
                                {
                                    totallate += 1;
                                }
                                isovertimefull = present.IsOvertimeFullDay;
                                if (isovertimefull)
                                {
                                    totalfullovertime += 1;
                                }
                                isovertimehalf = present.IsOvertimeHalfDay;
                                if (isovertimehalf)
                                {
                                    totalhalfovertime += 1;
                                }
                            }
                            else
                            {
                                totalabsent += 1;
                            }
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (paidlist != null)
                            {
                                if ((from x in paidlist where x.Date == todaydate.Date select x).Any() ? true : false && !ispresent)
                                {
                                    ispaidleave = true;
                                    totalpaidleave += 1;
                                }
                            }
                            if (WeekOff != null)
                            {
                                isweekoff = true;
                                totalweeklyoff += 1;
                            }
                            statuslist.Add(new Status()
                            {
                                Date = todaydate,
                                IsAbsent = ispresent != true,
                                IsLate = islate,
                                IsOvertimeFull = isovertimefull,
                                IsOvertimeHalf = isovertimehalf,
                                IsPaidLeave = ispaidleave,
                                IsPresent = ispresent,
                                IsWeeklyOff = isweekoff,
                                StatusString = BuilStatusString(ispresent, !ispresent, isovertimefull, isovertimehalf, isweekoff, islate, ispaidleave)
                            });
                        }
                        else { }

                    }
                    for (var i = 1; i <= endDate.Day; i++) 
                    {
                        var todaydate = DateTime.Parse($"{endDate.Year}-{endDate.Month}-{i}");
                        if (joindate.Date <= todaydate.Date)
                        {
                            totaldays += 1;
                            var isweekoff = false;
                            var ispresent = false;
                            var isovertimefull = false;
                            var isovertimehalf = false;
                            var ispaidleave = false;
                            var islate = false;
                            var present = (from x in c.OrgStaffsAttendancesDailies where x.ChekIN.Date == todaydate.Date && x.URId == (int)URId select x).FirstOrDefault();
                            if (present != null)
                            {
                                ispresent = true;
                                totalpresent += 1;
                                islate = present.Lateby != new TimeSpan() ? true : false;
                                if (islate)
                                {
                                    totallate += 1;
                                }
                                isovertimefull = present.IsOvertimeFullDay;
                                if (isovertimefull)
                                {
                                    totalfullovertime += 1;
                                }
                                isovertimehalf = present.IsOvertimeHalfDay;
                                if (isovertimehalf)
                                {
                                    totalhalfovertime += 1;
                                }
                            }
                            else
                            {
                                totalabsent += 1;
                            }
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (paidlist != null)
                            {
                                if ((from x in paidlist where x.Date == todaydate.Date select x).Any() ? true : false && !ispresent)
                                {
                                    ispaidleave = true;
                                    totalpaidleave += 1;
                                }
                            }
                            if (WeekOff != null)
                            {
                                isweekoff = true;
                                totalweeklyoff += 1;
                            }
                            statuslist.Add(new Status()
                            {
                                Date = todaydate,
                                IsAbsent = ispresent != true,
                                IsLate = islate,
                                IsOvertimeFull = isovertimefull,
                                IsOvertimeHalf = isovertimehalf,
                                IsPaidLeave = ispaidleave,
                                IsPresent = ispresent,
                                IsWeeklyOff = isweekoff,
                                StatusString = BuilStatusString(ispresent, !ispresent, isovertimefull, isovertimehalf, isweekoff, islate, ispaidleave)
                            });
                        }
                        else { }
                    }
                }
                statisticsByDateRange.Absent =totalabsent;
                statisticsByDateRange.EndDate=endDate.Date;
                statisticsByDateRange.Late=totallate;
                statisticsByDateRange.Name=(from x in c.SubUserOrganisations where x.URId==URId select x.SubUser.SubUsersDetail.FullName).FirstOrDefault();
                statisticsByDateRange.FullOverTime=totalfullovertime;
                statisticsByDateRange.HalfOverTime = totalhalfovertime;
                statisticsByDateRange.PaidLeave=totalpaidleave;
                statisticsByDateRange.Present=totalpresent;
                statisticsByDateRange.StartDate=startDate.Date;
                statisticsByDateRange.Status=statuslist;
                statisticsByDateRange.TotalDays=totaldays;
                statisticsByDateRange.URId=URId;
                statisticsByDateRange.WeeklyOff=totalweeklyoff;

            return new Result()
            {
                Status=Result.ResultStatus.success,
                Message="",
                Data=statisticsByDateRange,
            };
            }
        }
        public string BuilStatusString(bool ispresent, bool isabsent, bool isovertimefull,bool isovertimehalf, bool isweeklyoff, bool islate, bool ispaidleave)
        {
            StringBuilder result = new StringBuilder();
            if (ispresent)
            {
                result.Append("P");
                if (isweeklyoff)
                {
                    result.Append(" |WO");
                }
                if (isovertimefull)
                {
                    result.Append(" |OT-F");
                }
                if (isovertimehalf)
                {
                    result.Append(" |OT-H");
                }
                if (islate)
                {
                    result.Append(" |L");
                }
            }
            else 
            {
                result.Append("A");
                if (isweeklyoff) 
                {
                    result.Append(" |WO");
                }
                else if (ispaidleave) 
                {
                    result.Append(" |PL");
                }
            }
            return result.ToString();
        }*/
    }
}
    
