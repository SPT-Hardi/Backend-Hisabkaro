using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Statistics
    {
        public Result Get(int URId,DateTime date) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
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
                                          Name = obj.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                          ImagePath = obj.SubUserOrganisation.SubUser.SubUsersDetail.FileId == null ? null : obj.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
                     
                                      }).ToList();
                    foreach (var item in presentlist)
                    {
                        var lateby = new TimeSpan();
                        //var today = date.ToString("dd/MM/yyyy");
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
                            Status = "Absent",
                            LateBy = lateby.ToString(@"hh\:mm"),
                            Name = item.Name,
                            ImagePath = item.ImagePath,
                        });

                    }
                    var statistics = new Models.Employer.Organization.Staff.Attendance.Statistic()
                    {
                        TotalEmployee = totalemp.Count(),
                        Present = presentlist.Count(),
                        Absent = absentlist.Count(),
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