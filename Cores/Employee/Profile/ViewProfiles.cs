using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Profile
{
    public class ViewProfiles
    {
        public Result Get(int UserId)
        {
            using(DBContext c = new DBContext())
            {
                var profile = (from x in c.SubUsers
                               where x.UId == UserId
                               select new
                               {
                                   About = x.EmpResumeAbouts.Select(x => x.About),
                                   WorkExperience = (from y in x.EmpResumeWorkExperiences
                                                     where y.UId == x.UId
                                                     select new
                                                     {
                                                         Title = y.JobTitle,
                                                         Name = y.OrganizationName,
                                                         Experience = $"{Math.Floor((Math.Floor(((y.StartDate.Value.Date - y.EndDate.Value.Date).TotalDays) / 30)) / 12)}year{Math.Floor((Math.Floor(((y.StartDate.Value.Date - y.EndDate.Value.Date).TotalDays) / 30)) % 12)}month"
                                                     }).ToList(),
                                   Education = (from y in x.EmpResumeEducations
                                                where y.UId == x.UId
                                                select new
                                                {
                                                    CollegeName = y.InstituteName,
                                                    Degree = y.EducationSteamName,
                                                    Education = y.StartDate + " to " + y.EndDate,
                                                }).ToList(),
                                   Skill = (from y in x.EmpResumeSkills
                                            where y.UId == x.UId
                                            select new
                                            {
                                                Skill = y.SkillName
                                            }).ToList(),
                                   Certification = (from y in x.EmpResumeOtherCertificates
                                                    where y.UId == x.UId
                                                    select new
                                                    {
                                                        Name = y.CertificateName,
                                                        Duration = y.StartDate + " to " + y.EndDate,
                                                    }).ToList(),
                                   Contact = (from y in c.SubUsersDetails
                                              where y.UId == x.UId
                                              select new
                                              {
                                                  Address = y.CommonContactAddress.City + "," + y.CommonContactAddress.State,
                                                  EmailId = y.Email,
                                                  ContactNo = x.MobileNumber,
                                              }).ToList(),
                               }).ToList();


                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View Profile"),
                    Data = profile
                };
            }
        }
    }
}
