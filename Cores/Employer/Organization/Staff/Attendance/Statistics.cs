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
                    var totalemp = (from obj in c.SubUserOrganisations
                                    join obj1 in c.DevOrganisationsStaffs
                                    on obj.URId equals obj1.URId
                                    where obj.OId== findorg.OId && obj.RId == findstaffroleid.RId
                                    select obj1).ToList();
                    foreach (var item in totalemp) 
                    {
                        var lateby = new TimeSpan();
                        var todaydate = date.Date;
                        var today = date.ToString("dd/MM/yyyy");
                        var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.URId == item.URId && x.ChekIN.Value.Date==todaydate).SingleOrDefault();
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
                                AttendanceDate=todaydate,
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
                        TotalEmployee=totalemp.Count(),
                        Present=presentcount,
                        Absent=totalemp.Count()-presentcount,
                        Late=latecount,
                        WeeklyOff=0,
                    };
                
                    
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance-statistics get successfully!",
                        Data = new Models.Employer.Dashboard.Attendance.Report() 
                        {
                            statistics=statistics,
                            attendanceLists=attendancelist,
                        }
                    };
                }
            }
        }
    }
}
