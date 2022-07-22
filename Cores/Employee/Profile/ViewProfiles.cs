using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employee.Resume;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Profile
{
    public class ViewProfiles
    {
        public Result Get(object UId) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                if (user == null) 
                {
                    throw new ArgumentException("user not exist!");
                }
                var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                var res = new 
                {
                    AddressId =profile.AddressId,
                    CurrentSalary =profile.CurrentSalary,
                    Email =profile.Email,
                    EnglishLevel =new IntegerNullString() { Id=profile.SubFixedLookup_EnglishLevelId.FixedLookupId,Text= profile.SubFixedLookup_EnglishLevelId.FixedLookup},
                    IsVisibleToBusinessOwner =profile.IsVisibleToBussinessOwner,
                    MobileNumber =profile.MobileNumber,
                    Name =profile.FullName,
                    ProfileId =profile.ProfileId,
                    SalaryType =new IntegerNullString() { Id=profile.SubFixedLookup_SalaryTypeId.FixedLookupId,Text=profile.SubFixedLookup_SalaryTypeId.FixedLookup}
                };
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "user profile get successfully!",
                    Data = res
                };
            }
        }
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

     /*   public Result Get(object UserId)
        {
            using (DBContext c = new DBContext())
            {
                DashboardView userprofile = new DashboardView();
                var about = c.EmpResumeAbouts.Where(x => x.UId == (int)UserId).SingleOrDefault();
                if(about != null)
                {
                    userprofile.about.EmpResumeAboutId = about.EmpResumeAboutId;
                    userprofile.about.AboutText = about.About;
                }

                var experiencelist = c.EmpResumeWorkExperiences.Where(x => x.UId == (int)UserId).ToList();
                foreach (var item in experiencelist)
                {
                    var totaldays = (item.EndDate - item.StartDate).TotalDays;
                    var totalmonths = Math.Floor(totaldays / 30);
                    userprofile.experiences.Add(new ViewExperience()
                    {
                        EmpResumeWorkExperienceId = item.EmpResumeWorkExperienceId,
                        JobTitle = item.JobTitle,
                        OrganizationName = item.OrganizationName,
                        StartDate =item.StartDate, //$"{item.StartDate.ToString("MMMM").Substring(0, 3)} {item.StartDate.Year}",
                        EndDate = item.EndDate,//$"{item.EndDate.ToString("MMMM").Substring(0, 3)} {item.EndDate.Year}",
                        TotalDuration = $"{Math.Floor(totalmonths / 12)}yr {Math.Floor(totalmonths % 12)}mon",
                        WorkFrom = item.WorkFrom,
                    });
                }

                var educationlist = c.EmpResumeEducations.Where(x => x.UId == (int)UserId).ToList();
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
                        StartDate=item.StartDate,
                        EndDate=item.EndDate,
                        TotalDuration = syear + "-" + eyear,
                    });
                }


                var skills = c.EmpResumeSkills.Where(x => x.UId == (int)UserId).ToList();
                foreach (var item in skills) 
                {
                    userprofile.skills.Add(new SkillDetails()
                    {
                        EmpResumeSkillId=item.EmpResumeSkillId,
                        SkillName=item.SkillName,
                    });
                }

                var certificates = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UserId).ToList();
                foreach (var item in certificates) 
                {
                    var totaldays = (item.EndDate - item.StartDate).TotalDays;
                    var totalmonths = Math.Floor(totaldays / 30);
                    userprofile.otherCertificates.Add(new ViewOtherCertificate()
                    {
                        CertificateName=item.CertificateName,
                        EmpResumeOtherCertificateId=item.EmpResumeOtherCertificateId,
                        CertificateFGUId=item.CertificateFileId==null ? null : item.CommonFile.FGUID,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Duration = $"{Math.Floor(totalmonths / 12)}yr {Math.Floor(totalmonths % 12)}mons",
                    });
                }

                var contact = (from obj in c.SubUsersDetails
                               join obj1 in c.SubUsers
                               on obj.UId equals obj1.UId
                               where obj.UId == (int)UserId
                               select new ViewContact()
                               {
                                   UId = obj.UId,
                                   Address = obj.CommonContactAddress.City + ", " + obj.CommonContactAddress.State ,
                                   Email = obj.Email,
                                   MobileNumber = obj1.MobileNumber,

                               }).SingleOrDefault();
                userprofile.contact = contact;
                userprofile.UId = (int)UserId;
              
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View Profile"),
                    Data = userprofile,
                };
            }
        }*/
    }
}
