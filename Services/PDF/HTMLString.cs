﻿using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIsabKaro.Services.PDF
{
    public class HTMLString
    {
        public static string GetHTMLStringForResume(int UId)
        {
            using (DBContext c = new DBContext())
            {
               
                if ((int)UId == 0) 
                {
                    throw new ArgumentException("Not authorized!");
                }
                var MobileNumber = (from x in c.SubUsers where x.UId == (int)UId 
                                    select x.MobileNumber).FirstOrDefault();
                var UserDetails = (from x in c.SubUsersDetails where x.UId == (int)UId select new
                {
                    Name=x.FullName,
                    Email=x.Email,
                    City=x.CommonContactAddress==null ? null : x.CommonContactAddress.City,
                    State=x.CommonContactAddress==null ?null : x.CommonContactAddress.State,
                }).FirstOrDefault();
                var About = (from x in c.EmpResumeAbouts where x.UId == (int)UId select x.About).FirstOrDefault();
                var WorkExperience = (from x in c.EmpResumeWorkExperiences
                                      where x.UId == (int)UId
                                      select new
                                      {
                                          Designation=x.JobTitle,
                                          CompanyName=x.OrganizationName,
                                          Duration = new Cores.Common.StringFormators().CountDuration(x.StartDate,x.EndDate),
                                      }).ToList();
                var Education = (from x in c.EmpResumeEducations where x.UId == (int)UId select new
                {
                    StreamName=x.SubFixedLookup.FixedLookup,
                    InstituteName=x.InstituteName,
                    Duration= new Cores.Common.StringFormators().CountDuration(x.StartDate, x.EndDate),
                }).ToList();
                var Skills = (from x in c.EmpResumeSkills where x.UId == (int)UId select new
                {
                    SkillName=x.SkillName
                }).ToList();
                var OtherCertificate = (from x in c.EmpResumeOtherCertificates
                                        where x.UId == (int)UId
                                        select new
                                        {
                                            CertificateName=x.CertificateName,
                                            Duration= new Cores.Common.StringFormators().CountDuration(x.StartDate, x.EndDate),
                                        }).ToList();

                var sb = new StringBuilder();
                sb.Append($@"
                              <html>
                              <head> 
                              </head>
                              <body>
                             <div class='left'></div>
                              <div class='stuff'>
                              <br><br>
                             <h1>{UserDetails.Name}</h1>
                              <br/>
                              <p class='head'>{UserDetails.Email}</p>
                              <p class='head'>{MobileNumber}</p>
                              <p class='head'>{UserDetails.City},{UserDetails.State}</p><br/>
                              <h2>About</h2>
                                <hr/>
                                <p class='head'>{About}</p>
                                <br>
                                <h2> Work Experience </h2>
                                <hr/>

                          ");
                foreach (var x in WorkExperience)
                {
                    sb.Append($@"
                                 <h3>{x.Designation}</h3>
                                 <p class='title'>{x.CompanyName}</p>
                                 <p class='head'>{x.Duration}</p>
                                 <br>  
                              ");
                }
                sb.Append(@"
                          <h2>Education</h2>
                          <hr/>
                          ");
                foreach (var x in Education)
                {
                    sb.Append($@"
                               <h3>{x.InstituteName}</h3>
                               <p class='title'>{x.StreamName}</p>
                               <p class='head'>{x.Duration}</p>
                               <br>
                               ");
                }
                sb.Append(@"
                           <h2>Skills</h2>
                             <hr/>
                             <p class='head'> 
                         ");
                foreach (var x in Skills) 
                {
                    sb.Append($@"
                                 <li>{x.SkillName}</li>
                               ");
                }
                sb.Append(@"
                             </p>
                             <br>
                             <h2>Certificate</h2>
                             <hr/>
                           ");
                foreach (var x in OtherCertificate) 
                {
                    sb.Append($@"
                               <h3>{x.CertificateName}</h3>
                                <p class='head'>{x.Duration}</p>
                                ");
                }
                sb.Append(@"
                               </div>
                               <div class='right'></div>
                                </body>
                                </html>
                          ");
                return sb.ToString();
            }
        }
        public static string GetHTMLStringForAttendanceReport(int URId, DateTime startDate, DateTime endDate)
        {
            using (DBContext c = new DBContext())
            {
                var sd = startDate.Date;
                var totaldaysofrecord = ((endDate.Date - startDate.Date).TotalDays)+1;
                var findorg = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();
                if (findorg == null)
                {
                    throw new ArgumentException("Organization not exist,(enter valid token)");
                }
                List<int> user = new List<int>();
                user.Add(10000062);
               user.Add(10000024);
                user.Add(10000025);
               user.Add(10000027);
                var findstaffroleid = c.SubRoles.Where(x => x.RoleName.ToLower() == "staff" && x.OId == findorg.OId).SingleOrDefault();
                if (findstaffroleid != null)
                {
                    /*var totalemp = (from obj in c.DevOrganisationsStaffs
                                    where obj.OId == findorg.OId && obj.SubUserOrganisation.RId == findstaffroleid.RId && obj.CreateDate <= endDate.Date
                                    select obj).ToList();
                    if (totalemp.Count() <= 0) 
                    {
                        throw new ArgumentException("Currently not any user exist in your Organization!");
                    }*/
                    var sb = new StringBuilder();
                    var sp = new StringBuilder();
                   
                    if (totaldaysofrecord <= 16)
                    {
                        sb.Append(@"
                                  <html>
                                  <head> 
                                  </head>
                                  <body>
                                      <div>
                                          <table>
                                          <tr>
                                            <th>Name</th>
                                ");
                        while (sd <= startDate.AddDays(15))
                        {
                            sb.Append($@"
                                    <th>{sd.ToString("dd-MM-yyyy")}</th>
                                  ");
                            sd = sd.AddDays(1);
                        };
                        sp.Append(@"
                                     <div>
                                     <table>
                                      <tr>
                                        <th>Name</th>
                                        <th>TotalDays</th>
                                        <th>Present</th>
                                        <th>Absent</th>
                                        <th>WeeklyOff</th>
                                        <th>Late/HalfDay</th>
                                        <th>PaidLeave</th>
                                        <th>P-OverTimeF</th>
                                        <th>P-OverTimeH</th>
                                      </tr>
                                   ");
                        sb.Append($@"
                                </tr>
                                ");
                        foreach (var x in user)
                        {
                            StatisticsByDateRange attendancelist = new HistoryByMonths().StatisticsByDateRange(x, startDate, startDate.AddDays(15)).Data;
                        List<Status> status = attendancelist.Status;
                        var count = totaldaysofrecord - attendancelist.TotalDays;
                        sb.Append(@"
                               <tr>
                               <td>Raj</td>
                              ");
                        /*sb.Append(@"
                              ");*/
                        foreach (var y in status)
                        {
                            while (count != 0)
                            {
                                sb.Append($@"
                                    <td>_</td>
                                   ");
                                count -= 1;
                            }
                            sb.Append($@"
                                    <td>{y.StatusString}</td>
                                   ");
                            //startDate.AddDays(1);
                        }
                        sb.Append(@"
                               </tr>
                              ");
                            
                        sp.Append($@"<tr>
                                      <td>Raj</td>
                                      <td>{attendancelist.TotalDays}</td>
                                      <td>{attendancelist.Present}</td>
                                      <td>{attendancelist.Absent}</td>
                                      <td>{attendancelist.WeeklyOff}</td>
                                      <td>{attendancelist.Late}</td>
                                      <td>{attendancelist.PaidLeave}</td>
                                      <td>{attendancelist.FullOverTime}</td>
                                      <td>{attendancelist.HalfOverTime}</td>
                                    </tr>
                                     ");
                        }
                        sb.Append(@"</table>");
                        sb.Append($@"
                                </div>
                                ");
                        sb.Append(sp);
                        sb.Append(@"</table>
                                   </div>");
                        sb.Append(@"
                               </body>
                            </html>");
                    }
                    else 
                    {
                        List<StatisticsByDateRange> userlist = new List<StatisticsByDateRange>();
                       /* var totalpresent = 0;
                        var totalabsent = 0;
                        var totalweekoff = 0;
                        var totalpaidleave = 0;
                        var totallate = 0;
                        var totaldays = 0;
                        var totalovertimefull = 0;
                        var totalovertimehalf = 0;*/
                        var remainrecord = totaldaysofrecord - 16;
                        sb.Append(@"
                                  <html>
                                  <head> 
                                  </head>
                                  <body>
                                      <div>
                                          <table>
                                          <tr>
                                            <th>Name</th>
                                ");
                        sp.Append(@"
                                     <div>
                                     <table>
                                      <tr>
                                        <th>Name</th>
                                        <th>TotalDays</th>
                                        <th>Present</th>
                                        <th>Absent</th>
                                        <th>WeeklyOff</th>
                                        <th>Late/HalfDay</th>
                                        <th>PaidLeave</th>
                                        <th>P-OverTimeF</th>
                                        <th>P-OverTimeH</th>
                                      </tr>
                                   ");
                        while (sd <= startDate.AddDays(15))
                        {
                            sb.Append($@"
                                    <th>{sd.ToString("dd-MM-yyyy")}</th>
                                  ");
                            sd = sd.AddDays(1);
                        };
                        sb.Append($@"
                                </tr>
                                ");

                        foreach (var x in user)
                        {
                            StatisticsByDateRange attendancelist = new HistoryByMonths().StatisticsByDateRange(x, startDate, sd.AddDays(-1)).Data;
                            userlist.Add(attendancelist);
                            List<Status> status = attendancelist.Status;
                            var count = 16 - attendancelist.TotalDays;
                            sb.Append(@"
                               <tr>
                               <td>Raj</td>
                              ");
                            foreach (var y in status)
                            {
                                while (count != 0)
                                {
                                    sb.Append($@"
                                    <td>_</td>
                                   ");
                                    count -= 1;
                                }
                                sb.Append($@"
                                    <td>{y.StatusString}</td>
                                   ");
                                //startDate.AddDays(1);
                            }
                            sb.Append(@"
                               </tr>
                              ");
                           /* totalabsent =attendancelist.Absent;
                            totalpresent =attendancelist.Present;
                            totaldays =attendancelist.TotalDays;
                            totalweekoff =attendancelist.WeeklyOff;
                            totallate =attendancelist.Late;
                            totalpaidleave =attendancelist.PaidLeave;
                            totalovertimehalf =attendancelist.HalfOverTime;
                            totalovertimefull =attendancelist.FullOverTime;*/
                            /*sp.Append($@"<tr>
                                      <td>Raj</td>
                                      <td>{attendancelist.TotalDays}</td>
                                      <td>{attendancelist.Present}</td>
                                      <td>{attendancelist.Absent}</td>
                                      <td>{attendancelist.WeeklyOff}</td>
                                      <td>{attendancelist.Late}</td>
                                      <td>{attendancelist.PaidLeave}</td>
                                      <td>{attendancelist.FullOverTime}</td>
                                      <td>{attendancelist.HalfOverTime}</td>
                                    </tr>
                                     ");*/
                        }
                            sb.Append(@"
                              </table>
                              ");
                            //------------------------------- after 16 col new table
                            sb.Append(@"<table>
                                <tr>
                                <th>Name</th>");
                        var remainsd = sd;
                        while (sd <= endDate.Date)
                        {
                            sb.Append($@"
                                    <th>{sd.ToString("dd-MM-yyyy")}</th>
                                  ");
                            sd = sd.AddDays(1);
                        };
                        sb.Append($@"
                                </tr>
                                ");
                        foreach (var x in user)
                        {
                            var attendancelist = (from y in userlist where y.URId == x select y).FirstOrDefault();
                            StatisticsByDateRange attendancelist17 = new HistoryByMonths().StatisticsByDateRange(x, remainsd, endDate).Data;
                            List<Status> status17 = attendancelist17.Status;
                            var count17 = remainrecord - attendancelist17.TotalDays;
                            sb.Append(@"
                               <tr>
                               <td>Raj</td>
                              ");
                            foreach (var y in status17)
                            {
                                while (count17 != 0)
                                {
                                    sb.Append($@"
                                    <td>_</td>
                                   ");
                                    count17 -= 1;
                                }
                                sb.Append($@"
                                    <td>{y.StatusString}</td>
                                   ");
                                //startDate.AddDays(1);
                            }
                            sb.Append(@"
                               </tr>
                              ");/*
                            totalabsent +=attendancelist17.Absent;
                            totalpresent +=attendancelist17.Present;
                            totaldays +=attendancelist17.TotalDays;
                            totalweekoff +=attendancelist17.WeeklyOff;
                            totallate += attendancelist17.Late;
                            totalpaidleave += attendancelist17.PaidLeave;
                            totalovertimehalf +=attendancelist17.HalfOverTime;
                            totalovertimefull +=attendancelist17.FullOverTime;*/
                            sp.Append($@"<tr>
                                      <td>Raj</td>
                                      <td>{attendancelist.TotalDays + attendancelist17.TotalDays}</td>
                                      <td>{attendancelist.Present+attendancelist17.Present}</td>
                                      <td>{attendancelist.Absent+attendancelist17.Absent}</td>
                                      <td>{attendancelist.WeeklyOff+attendancelist17.WeeklyOff}</td>
                                      <td>{attendancelist.Late+attendancelist17.Late}</td>
                                      <td>{attendancelist.PaidLeave+attendancelist17.PaidLeave}</td>
                                      <td>{attendancelist.FullOverTime+attendancelist17.FullOverTime}</td>
                                      <td>{attendancelist.HalfOverTime+attendancelist17.HalfOverTime}</td>
                                    </tr>
                                     ");
                        }
                        sb.Append(@"
                              </table>
                              ");
                        sb.Append(@"</div>");
                        sb.Append(sp);
                        sb.Append(@"</table>
                                     </div>");
                        sb.Append(@"
                                    </body>
                                 </html>
                              ");

                    }
                    return sb.ToString();
                }
                else 
                {
                    throw new ArgumentException("Currently not any user exist in your Organization!");
                }
            }
        }
    }
}