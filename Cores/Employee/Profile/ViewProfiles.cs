using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employee.Resume;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Profile
{
    public class ViewProfiles
    {
        //public Result Get(int UserId)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        var profile = (from x in c.SubUsers
        //                       where x.UId == UserId
        //                       select new
        //                       {
        //                           About = x.EmpResumeAbouts.Select(x => x.About),
        //                           WorkExperience = (from y in x.EmpResumeWorkExperiences
        //                                             where y.UId == x.UId
        //                                             select new
        //                                             {
        //                                                 Title = y.JobTitle,
        //                                                 Name = y.OrganizationName,
        //                                                 Experience = $"{Math.Floor((Math.Floor(((y.StartDate.Value.Date - y.EndDate.Value.Date).TotalDays) / 30)) / 12)}year{Math.Floor((Math.Floor(((y.StartDate.Value.Date - y.EndDate.Value.Date).TotalDays) / 30)) % 12)}month"
        //                                             }).ToList(),
        //                           Education = (from y in x.EmpResumeEducations
        //                                        where y.UId == x.UId
        //                                        select new
        //                                        {
        //                                            CollegeName = y.InstituteName,
        //                                            Degree = y.EducationSteamName,
        //                                            Education = y.StartDate + " to " + y.EndDate,
        //                                        }).ToList(),
        //                           Skill = (from y in x.EmpResumeSkills
        //                                    where y.UId == x.UId
        //                                    select new
        //                                    {
        //                                        Skill = y.SkillName
        //                                    }).ToList(),
        //                           Certification = (from y in x.EmpResumeOtherCertificates
        //                                            where y.UId == x.UId
        //                                            select new
        //                                            {
        //                                                Name = y.CertificateName,
        //                                                Duration = y.StartDate + " to " + y.EndDate,
        //                                            }).ToList(),
        //                           Contact = (from y in c.SubUsersDetails
        //                                      where y.UId == x.UId
        //                                      select new
        //                                      {
        //                                          Address = y.CommonContactAddress.City + "," + y.CommonContactAddress.State,
        //                                          EmailId = y.Email,
        //                                          ContactNo = x.MobileNumber,
        //                                      }).ToList(),
        //                       }).ToList();


        //        return new Result()
        //        {
        //            Status = Result.ResultStatus.success,
        //            Message = string.Format("View Profile"),
        //            Data = profile
        //        };
        //    }
        //    //}
        //}

        public Result Get(int UserId)
        {
            using (DBContext c = new DBContext())
            {
                DashboardView userprofile = new DashboardView();
                var about = c.EmpResumeAbouts.Where(x => x.UId == UserId).SingleOrDefault();

                userprofile.about.EmpResumeAboutId = about.EmpResumeAboutId;
                userprofile.about.AboutText = about.About;

                var experiencelist = c.EmpResumeWorkExperiences.Where(x => x.UId == UserId).ToList();
                foreach (var item in experiencelist)
                {
                    var totaldays = (item.EndDate - item.StartDate).Value.TotalDays;
                    var totalmonths = Math.Floor(totaldays / 30);
                    userprofile.experiences.Add(new ViewExperience()
                    {
                        EmpResumeWorkExperienceId = item.EmpResumeWorkExperienceId,
                        JobTitle = item.JobTitle,
                        OrganizationName = item.OrganizationName,
                        StartDate = $"{item.StartDate.Value.ToString("MMMM").Substring(0, 3)} {item.StartDate.Value.Year}",
                        EndDate = $"{item.EndDate.Value.ToString("MMMM").Substring(0, 3)} {item.EndDate.Value.Year}",
                        TotalDuration = $"{Math.Floor(totalmonths / 12)}yr {Math.Floor(totalmonths % 12)}mons",
                        WorkFrom = item.WorkFrom,
                    });
                }

                var educationlist = c.EmpResumeEducations.Where(x => x.UId == UserId).ToList();
                foreach (var item in educationlist)
                {
                    var syear = string.Format("{0 : yyyy}", item.StartDate);
                    var eyear = string.Format("{0 : yyyy}", item.EndDate);
                    userprofile.educations.Add(new EduDetail()
                    {
                        EmpResumeEducationId = item.EmpResumeEducationId,
                        InstituteName = item.InstituteName,
                        EducationName = new IntegerNullString() { Id = item.EducationNameId, Text = item.SubFixedLookup.FixedLookup },
                        EducationStreamName = item.EducationSteamName,
                        TotalDuration = syear + "-" + eyear,
                    });
                }


                var skills = c.EmpResumeSkills.Where(x => x.UId == UserId).ToList();
                foreach (var item in skills) 
                {
                    userprofile.skills.Add(new SkillDetails()
                    {
                        EmpResumeSkillId=item.EmpResumeSkillId,
                        SkillName=item.SkillName,
                    });
                }

                var certificates = c.EmpResumeOtherCertificates.Where(x => x.UId == UserId).ToList();
                foreach (var item in certificates) 
                {
                    var totaldays = (item.EndDate - item.StartDate).Value.TotalDays;
                    var totalmonths = Math.Floor(totaldays / 30);
                    userprofile.otherCertificates.Add(new ViewOtherCertificate()
                    {
                        EmpResumeOtherCertificateId=item.EmpResumeOtherCertificateId,
                        Duration= $"{Math.Floor(totalmonths / 12)}yr {Math.Floor(totalmonths % 12)}mons",
                    });
                }

                var contact = (from obj in c.SubUsersDetails
                               join obj1 in c.SubUsers
                               on obj.UId equals obj1.UId
                               where obj.UId == UserId
                               select new ViewContact()
                               {
                                   UId = obj.UId,
                                   Address = obj.CommonContactAddress.City + ", " + obj.CommonContactAddress.State ,
                                   Email = obj.Email,
                                   MobileNumber = obj1.MobileNumber,

                               }).SingleOrDefault();
                userprofile.contact = contact;
              
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View Profile"),
                    Data = userprofile,
                };
            }
        }
    }
}
