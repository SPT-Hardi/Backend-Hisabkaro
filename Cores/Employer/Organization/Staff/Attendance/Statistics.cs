using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Statistics
    {
        public Result Get(object URId,DateTime date) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {

                    int presentcount = 0;
                    int latecount = 0;
                    
                    var attendancelist = new List<Models.Employer.Organization.Staff.Attendance.AttendanceList>();
                    var statistics = new Models.Employer.Organization.Staff.Attendance.Statistic();
                    var findorg = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();

//------------------------------------------------------------------------------------------------------//
                    //remove after controller logic added
                   /* if (findorg.SubRole.RoleName.ToLower() != "admin")
                    {
                        throw new ArgumentException("You are not authorize!");
                    }*/
//------------------------------------------------------------------------------------------------------//
                    if (findorg == null)
                    {
                        throw new ArgumentException("Organization not exist,(enter valid token)");
                    }
                    var findstaffroleid = c.SubRoles.Where(x => x.RoleName.ToLower() == "staff" && x.OId == findorg.OId).SingleOrDefault();
                    if (findstaffroleid != null)
                    {
                        //throw new ArgumentException("Currently no staff in your organization!");


                        var totalemp = (from obj in c.DevOrganisationsStaffs
                                        where obj.OId == findorg.OId && obj.SubUserOrganisation.RId == findstaffroleid.RId && obj.CreateDate <= date.Date
                                        select obj).ToList();
                        var presentlist = (from obj in c.DevOrganisationsStaffs
                                           join obj1 in c.OrgStaffsAttendancesDailies
                                           on obj.URId equals obj1.URId
                                           where obj.OId == findorg.OId && obj1.ChekIN.Date == date.Date && obj.CreateDate <= date.Date
                                           select new
                                           {
                                               URId = obj1.URId,
                                               CheckIN = obj1.ChekIN,
                                               CheckOUT = obj1.CheckOUT,
                                               IsOvertime = obj1.IsOvertimeFullDay || obj1.IsOvertimeHalfDay,
                                               Lateby = obj1.Lateby,
                                               MarkLate = obj.DevOrganisationsShiftTime.MarkLate,
                                               Name = obj.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                               ImagePath = obj1.PhotoFileId == null ? (obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID) : obj1.CommonFile.FGUID,
                                           }).ToList();
                        //var absentlist = (from obj in totalemp
                        //                  join obj1 in presentlist
                        //                  on obj.URId equals obj1.URId
                        //                  where obj.URId != obj1.URId || obj1.URId == null
                        //                  select new
                        //                  {
                        //                      URId = obj.URId,
                        //                      CheckIN = new TimeSpan(),
                        //                      CheckOUT = new TimeSpan(),
                        //                      Name = obj.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                        //                      ImagePath = obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
                        //
                        //                  }).ToList();
                        
                        var absentlist = (from obj in totalemp
                                          where !presentlist.Any(x => x.URId == obj.URId)
                                          select new
                                          {
                                              URId = obj.URId,
                                              CheckIN = new TimeSpan(),
                                              CheckOUT = new TimeSpan(),
                                              Name = obj.NickName,
                                              PaidLeaveList=PaidLeaveList(obj.URId,date),
                                              ImagePath = obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID,
                                              WeekoffoneDay = (obj.WeekOffOneDay == null) ? false : obj.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == date.DayOfWeek.ToString().ToLower(),
                                              WeekoffsecondDay = (obj.WeekOffSecondDay == null) ? false : obj.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == date.DayOfWeek.ToString().ToLower(),
                                          }).ToList();
                        /* var overtime = (from obj in c.DevOrganisationsStaffs
                                         join obj1 in c.OrgStaffsAttendancesDailies
                                         on obj.URId equals obj1.URId
                                         where obj.OId == findorg.OId && obj1.ChekIN.Value.Date == date.Date && ((obj.WeekOffOneDay == null ? false : obj.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == date.DayOfWeek.ToString().ToLower()) || (obj.WeekOffSecondDay == null ? false : obj.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == date.DayOfWeek.ToString().ToLower()))
                                         select obj).ToList();
                         var overtimecount = overtime.Count();*/
                        var overtimelist = (from x in presentlist where x.IsOvertime == true select x).ToList();
                        var weekofflist = (from obj in absentlist
                                           where (obj.WeekoffoneDay || obj.WeekoffsecondDay) == true
                                           select obj
                                         ).ToList();
                        foreach (var item in presentlist)
                        {

                            //var today = date.ToString("dd/MM/yyyy");
                            presentcount += 1;

                            if (item.Lateby != new TimeSpan())
                            {
                                latecount += 1;
                            }

                            attendancelist.Add(new Models.Employer.Organization.Staff.Attendance.AttendanceList()
                            {
                                URId = (int)item.URId,
                                AttendanceDate = date.Date,
                                CheckIN = item.CheckIN.TimeOfDay.ToString(@"hh\:mm"),
                                CheckOUT = item.CheckOUT == null ? null : item.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                                Status = item.IsOvertime == true ? "Overtime" : "Present",
                                LateBy = item.Lateby == new TimeSpan() ? null : item.Lateby.ToString(@"hh\:mm"),
                                Name = item.Name,
                                ImagePath = item.ImagePath,
                                IsAbsent=false,
                                IsLate=item.Lateby==new TimeSpan()?false:true,
                                IsOverTime= item.IsOvertime,
                                IsPaidLeave=false,
                                IsWeeklyOff= item.IsOvertime ,
                                IsPresent=true,
                            });

                        }
                        foreach (var item in absentlist)
                        {
                            var lateby = new TimeSpan();
                            //var today = date.ToString("dd/MM/yyyy");

                            attendancelist.Add(new Models.Employer.Organization.Staff.Attendance.AttendanceList()
                            {
                                URId = (int)item.URId,
                                AttendanceDate = date.Date,
                                CheckIN = item.CheckIN.ToString(@"hh\:mm"),
                                CheckOUT = item.CheckOUT.ToString(@"hh\:mm"),
                                Status = (item.WeekoffoneDay || item.WeekoffsecondDay) ? "WeekOff" : "Absent",
                                LateBy = lateby.ToString(@"hh\:mm"),
                                Name = item.Name,
                                ImagePath = item.ImagePath,
                                IsAbsent = true,
                                IsLate = false,
                                IsOverTime = false,
                                IsPaidLeave = item.PaidLeaveList==null ? false : ((from x in item.PaidLeaveList where x.Date==date.Date select x).Any()?true:false),
                                IsWeeklyOff = (item.WeekoffoneDay || item.WeekoffsecondDay)?true:false,
                                IsPresent = false,
                            });


                        }

                        statistics.Organization = new IntegerNullString() { Id = findorg.OId, Text = findorg.DevOrganisation.OrganisationName };
                        statistics.TotalEmployee = totalemp.Count();
                        statistics.Present = presentlist.Count();
                        statistics.Absent = absentlist.Count() - weekofflist.Count();
                        statistics.Late = latecount;
                        statistics.WeeklyOff = weekofflist.Count();
                        statistics.Overtime = overtimelist.Count();
                        
                    }
                    else 
                    {
                       
                        statistics.Organization = new IntegerNullString() { Id = findorg.OId, Text = findorg.DevOrganisation.OrganisationName };
                        statistics.TotalEmployee = 0;
                        statistics.Present = 0;
                        statistics.Absent = 0;
                        statistics.Late = 0;
                        statistics.WeeklyOff = 0;
                        statistics.Overtime = 0;

                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance-statistics get successfully!",
                        Data = new Models.Employer.Dashboard.Attendance.Report()
                        {
                            statistics = statistics,
                            attendanceLists = attendancelist,
                        }
                    };
                }


            }

        }
        public List<DateTime> PaidLeaveList(int URId,DateTime date) 
        {
            using (DBContext c = new DBContext()) 
            {
                var paidleave = (from x in c.OrgStaffsLeaveApplications where x.StaffURId == URId && x.StartDate.Month == date.Month && x.StartDate.Year == date.Year select new { x.StartDate,x.PaidDays,x.EndDate}).FirstOrDefault();
                List<DateTime> paidleavelist = new List<DateTime>();
                if (paidleave != null)
                {
                    var count = paidleave.PaidDays;
                    /*for (var i = 0; i < paidleave.PaidDays; i++) 
                    {
                        var WeekOff = (from x in c.DevOrganisationsStaffs
                                       where
                                       x.URId == (int)URId &&
                                       ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                       select x).FirstOrDefault();
                        paidleavelist.Add(paidleave.StartDate.AddDays(i));
                    }*/
                    if (paidleave.StartDate.Month == paidleave.EndDate.Month && paidleave.StartDate.Year == paidleave.EndDate.Year)
                    {
                        for (var i = paidleave.StartDate.Day; i <= paidleave.EndDate.Day; i++)
                        {
                            if (count == 0) break;
                            var todaydate = DateTime.Parse($"{paidleave.StartDate.Year}-{paidleave.StartDate.Month}-{i}");
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (WeekOff == null) 
                            {
                                paidleavelist.Add(todaydate.Date);
                                count -= 1;
                            }
                        }
                    }
                    else 
                    {
                        var daysinmonth = DateTime.DaysInMonth(paidleave.StartDate.Year, paidleave.StartDate.Month);
                        for (var i = paidleave.StartDate.Day; i <= daysinmonth; i++) 
                        {
                            if (count == 0) break;
                            var todaydate = DateTime.Parse($"{paidleave.StartDate.Year}-{paidleave.StartDate.Month}-{i}");
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (WeekOff == null)
                            {
                                paidleavelist.Add(todaydate.Date);
                                count -= 1;
                            }
                        }
                        for (var i = 1; i <= paidleave.EndDate.Day; i++) 
                        {
                            if (count == 0) break;
                            var todaydate = DateTime.Parse($"{paidleave.EndDate.Year}-{paidleave.EndDate.Month}-{i}");
                            var WeekOff = (from x in c.DevOrganisationsStaffs
                                           where
                                           x.URId == (int)URId &&
                                           ((x.WeekOffOneDay == null && x.WeekOffSecondDay == null) ? false : (x.SubFixedLookup_WeekOffOneDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower() || x.SubFixedLookup_WeekOffSecondDay.FixedLookup.ToLower() == todaydate.DayOfWeek.ToString().ToLower()))
                                           select x).FirstOrDefault();
                            if (WeekOff == null)
                            {
                                paidleavelist.Add(todaydate.Date);
                                count -= 1;
                            }
                        }
                    }
                    return paidleavelist;
                }
                else 
                {
                    return null;
                }
            }
        }

    }
}
/*using (DBContext c = new DBContext())
{
    int presentcount = 0;
    int latecount = 0;

    var attendancelist = new List<Models.Employer.Organization.Staff.Attendance.AttendanceList>();
    var findorg = c.SubUserOrganisations.Where(x => x.URId == URId).SingleOrDefault();
    if (findorg == null)
    {
        throw new ArgumentException("Organization not exist,(enter valid token)");
    }
    var findstaffroleid = c.SubRoles.Where(x => x.RoleName.ToLower() == "staff" && x.OId == findorg.OId).SingleOrDefault();
    if (findstaffroleid == null)
    {
        throw new ArgumentException("Currently no staff in your organization!");
    }
    var totalemp = (from obj in c.DevOrganisationsStaffs
                    where obj.OId == findorg.OId && obj.SubUserOrganisation.RId == findstaffroleid.RId
                    select obj).ToList();
    var presentlist = (from obj in c.DevOrganisationsStaffs
                       join obj1 in c.OrgStaffsAttendancesDailies
                       on obj.URId equals obj1.URId
                       where obj.OId == findorg.OId && obj1.ChekIN.Value.Date == date.Date
                       select new
                       {
                           URId = obj1.URId,
                           CheckIN = obj1.ChekIN,
                           CheckOUT = obj1.CheckOUT,
                           IsLate = obj1.IsLate,
                           MarkLate = obj.DevOrganisationsShiftTime.MarkLate,
                           Name = obj.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                           ImagePath = obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
                       }).ToList();
    var absentlist = (from obj in totalemp
                      join obj1 in presentlist
                      on obj.URId equals obj1.URId
                      where obj.URId != obj1.URId || obj1.URId == null
                      select new
                      {
                          URId = obj.URId,
                          CheckIN = new TimeSpan(),
                          CheckOUT = new TimeSpan(),
                          Name = obj.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                          ImagePath = obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,

                      }).ToList();

    foreach (var item in presentlist)
    {
        var lateby = new TimeSpan();
        var today = date.ToString("dd/MM/yyyy");
        presentcount += 1;

        if (item.IsLate == true)
        {
            latecount += 1;
            lateby = item.CheckIN.Value.TimeOfDay - (TimeSpan)item.MarkLate;
        }
        attendancelist.Add(new Models.Employer.Organization.Staff.Attendance.AttendanceList()
        {
            URId = (int)item.URId,
            AttendanceDate = date.Date,
            CheckIN = item.CheckIN.Value.TimeOfDay.ToString(@"hh\:mm"),
            CheckOUT = item.CheckOUT == null ? null : item.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
            Status = "Present",
            LateBy = lateby.ToString(@"hh\:mm"),
            Name = item.Name,
            ImagePath = item.ImagePath,
        });

    }

    var statistics = new Models.Employer.Organization.Staff.Attendance.Statistic()
    {
        TotalEmployee = totalemp.Count(),
        Present = presentcount,
        // Absent = totalemp.Count() - presentcount,
        Late = latecount,
        WeeklyOff = 0,
    };
    return new Result()
    {
        Status = Result.ResultStatus.success,
        Message = "Staff attendance-statistics get successfully!",
        Data = new Models.Employer.Dashboard.Attendance.Report()
        {
            statistics = statistics,
            attendanceLists = attendancelist,
        }
    };
}

/*using (DBContext c = new DBContext())
{
    int presentcount = 0;
    int latecount = 0;

    var attendancelist = new List<Models.Employer.Organization.Staff.Attendance.AttendanceList>();
    var findorg = c.SubUserOrganisations.Where(x => x.URId == URId).SingleOrDefault();
    if (findorg == null)
    {
        throw new ArgumentException("Organization not exist,(enter valid token)");
    }
    var findstaffroleid = c.SubRoles.Where(x => x.RoleName.ToLower() == "staff" && x.OId == findorg.OId).SingleOrDefault();
    if (findstaffroleid == null)
    {
        throw new ArgumentException("Currently no staff in your organization!");
    }
    var totalemp = (from obj in c.SubUserOrganisations
                    join obj1 in c.DevOrganisationsStaffs
                    on obj.URId equals obj1.URId
                    where obj.OId == findorg.OId && obj.RId == findstaffroleid.RId
                    select obj1).ToList();
    var presentlist = c.OrgStaffsAttendancesDailies.Where(x => x.SubUserOrganisation.OId == findorg.OId && x.SubUserOrganisation.RId == findstaffroleid.RId).ToList();



    foreach (var item in totalemp)
    {
        var lateby = new TimeSpan();
        var todaydate = date.Date;
        var today = date.ToString("dd/MM/yyyy");
        var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.URId == item.URId && x.ChekIN.Value.Date == todaydate).SingleOrDefault();
        if (checkpresent != null)
        {
            presentcount += 1;

            if (checkpresent.IsLate == true)
            {
                latecount += 1;
                lateby = checkpresent.ChekIN.Value.TimeOfDay - (TimeSpan)item.DevOrganisationsShiftTime.MarkLate;
            }
            attendancelist.Add(new Models.Employer.Organization.Staff.Attendance.AttendanceList()
            {
                URId = item.URId,
                AttendanceDate = todaydate,
                CheckIN = checkpresent.ChekIN.Value.TimeOfDay.ToString(@"hh\:mm"),
                CheckOUT = checkpresent.CheckOUT == null ? null : checkpresent.CheckOUT.Value.TimeOfDay.ToString(@"hh\:mm"),
                Status = "Present",
                LateBy = lateby.ToString(@"hh\:mm"),
                Name = item.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                ImagePath = item.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : item.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
            });

        }
        else
        {

            attendancelist.Add(new Models.Employer.Organization.Staff.Attendance.AttendanceList()
            {
                URId = item.URId,
                AttendanceDate = todaydate,
                CheckIN = "00:00",
                CheckOUT = "00:00",
                Status = "Absent",
                LateBy = "00:00",
                Name = item.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                ImagePath = item.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : item.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
            });
        }
    }
    var statistics = new Models.Employer.Organization.Staff.Attendance.Statistic()
    {
        TotalEmployee = totalemp.Count(),
        Present = presentcount,
        Absent = totalemp.Count() - presentcount,
        Late = latecount,
        WeeklyOff = 0,
    };


    return new Result()
    {
        Status = Result.ResultStatus.success,
        Message = "Staff attendance-statistics get successfully!",
        Data = new Models.Employer.Dashboard.Attendance.Report()
        {
            statistics = statistics,
            attendanceLists = attendancelist,
        }
    };
}
*/