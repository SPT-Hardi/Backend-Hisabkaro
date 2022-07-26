using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HIsabKaro.Cores.Employer.Organization.Job.ER_JobDetails;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_ViewJobs
    {
        public Result List(object UId) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var res = (from x in c.EmprJobs
                           where x.JobStatusId!=(int)JobStatus.Disable && x.JobStatusId!=(int)JobStatus.Remove
                           orderby x.PostDate descending
                           select new
                           {
                               JobId = x.JobId,
                               IsYouShortListed= (from z in c.EmprApplicantShortListDetails where z.ApplicantUId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouApplied =(from z in c.EmpApplyJobDetails where z.UId==(int)UId && z.JobId==x.JobId select x).Any()? true:false,
                               IsYouBookMarked= (from z in c.EmpBookmarkJobsDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               MobileNumber =x.MobileNumber,
                               JobTitle = x.Title,
                               JobType = (from t in c.EmprJobTypes
                                          where t.JobId == x.JobId
                                          select new { Type = t.Type }).ToList(),
                               Image = x.DevOrganisation.CommonFile.FGUID,
                               MinSalary = x.MinSalary,
                               MaxSalary = x.MaxSalary,
                               Organization = new IntegerNullString() { Id = x.DevOrganisation.OId, Text = x.DevOrganisation.OrganisationName },
                               Applied = (from y in c.EmpApplyJobDetails
                                          where y.JobId == x.JobId
                                          select y.UId).Count(),
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Status = x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                               Address = new { City = x.DevOrganisation.CommonContactAddress.City, State = x.DevOrganisation.CommonContactAddress.State } 
                           }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "List of jobs get successfully!",
                    Data = res
                };
            }
        }
        public Result One(object UId,int Jid) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.JobStatusId != (int)JobStatus.Remove && o.JobStatusId != (int)JobStatus.Disable);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                var res = (from x in c.EmprJobs
                           where x.JobId == Jid
                           select new
                           {
                               JobId = x.JobId,
                               IsYouShortListed = (from z in c.EmprApplicantShortListDetails where z.ApplicantUId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouApplied = (from z in c.EmpApplyJobDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouBookMarked = (from z in c.EmpBookmarkJobsDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               SalaryType = new IntegerNullString() { Id = x.SubFixedLookup_SalaryTypeId.FixedLookupId, Text = x.SubFixedLookup_SalaryTypeId.FixedLookup, },
                               AddressId = x.AddressId,
                               Comment = x.Comment,
                               Description = x.Description,
                               Email = x.Email,
                               EndDate = x.EndDate,
                               EnglishLevel = x.EmprJobEnglishLevels.ToList().Select(z => new { level = z.EnglishLevel }).ToList(),
                               OtherLanguage = x.EmprJobOtherLanguages.ToList().Select(z => new { language = z.OtherLanguage }).ToList(),
                               ExperienceLevels = x.EmprJobExperienceLevels.ToList().Select(z => new { level = z.ExperienceLevel }).ToList(),
                               IncentiveType = new IntegerNullString() { Id = x.SubFixedLookup_IncentiveTypeId.FixedLookupId, Text = x.SubFixedLookup_IncentiveTypeId.FixedLookup, },
                               JobEndDate = x.EndDate,
                               jobSkill = x.EmprJobSkills.ToList().Select(z => new { skill = z.Skill }).ToList(),
                               JobStartTime = x.JobStartTime,
                               jobType = x.EmprJobTypes.ToList().Select(z => new { type = z.Type }).ToList(),
                               MaxIncentive = x.MaxIncentive,
                               MaxSalary = x.MaxSalary,
                               MinIncentive = x.MinIncentive,
                               MinSalary = x.MinSalary,
                               MobileNumber = x.MobileNumber,
                               Status = x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                               Organization = new IntegerNullString() { Id = x.DevOrganisation.OId, Text = x.DevOrganisation.OrganisationName, },
                               Title = x.Title,
                               Applied = (from y in c.EmpApplyJobDetails
                                          where y.JobId == x.JobId
                                          select y.UId).Count(),
                           }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Job details get successfully!",
                    Data = res,
                };
            }
        }
        public Result Search_ByLocation(object UId,string keyword) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var res = (from x in c.EmprJobs
                           where x.JobStatusId != (int)JobStatus.Disable && x.JobStatusId != (int)JobStatus.Remove && (x.CommonContactAddress.AddressLine1.Contains(keyword) || x.CommonContactAddress.City.Contains(keyword) || x.CommonContactAddress.State.Contains(keyword))
                           orderby x.PostDate descending
                           select new
                           {
                               JobId = x.JobId,
                               IsYouShortListed = (from z in c.EmprApplicantShortListDetails where z.ApplicantUId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouApplied = (from z in c.EmpApplyJobDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouBookMarked = (from z in c.EmpBookmarkJobsDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               MobileNumber = x.MobileNumber,
                               JobTitle = x.Title,
                               JobType = (from t in c.EmprJobTypes
                                          where t.JobId == x.JobId
                                          select new { Type = t.Type }).ToList(),
                               Image = x.DevOrganisation.CommonFile.FGUID,
                               MinSalary = x.MinSalary,
                               MaxSalary = x.MaxSalary,
                               Organization = new IntegerNullString() { Id = x.DevOrganisation.OId, Text = x.DevOrganisation.OrganisationName },
                               Applied = (from y in c.EmpApplyJobDetails
                                          where y.JobId == x.JobId
                                          select y.UId).Count(),
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Status = x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                               Address = new { City = x.DevOrganisation.CommonContactAddress.City, State = x.DevOrganisation.CommonContactAddress.State } 
                           }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Jobs list get successfully!",
                    Data = res
                };
            }
        }
        public Result Search_BySector_Company_JobTitle(object UId, string keyword)
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var res = (from x in c.EmprJobs
                           where x.JobStatusId != (int)JobStatus.Disable && x.JobStatusId != (int)JobStatus.Remove && ( x.DevOrganisation.OrganisationName.Contains(keyword)  || x.Title.Contains(keyword) || x.DevOrganisation.SubFixedLookup.FixedLookup.Contains(keyword))
                           orderby x.PostDate descending
                           select new
                           {
                               JobId = x.JobId,
                               IsYouShortListed = (from z in c.EmprApplicantShortListDetails where z.ApplicantUId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouApplied = (from z in c.EmpApplyJobDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               IsYouBookMarked = (from z in c.EmpBookmarkJobsDetails where z.UId == (int)UId && z.JobId == x.JobId select x).Any() ? true : false,
                               MobileNumber = x.MobileNumber,
                               JobTitle = x.Title,
                               JobType = (from t in c.EmprJobTypes
                                          where t.JobId == x.JobId
                                          select new { Type = t.Type }).ToList(),
                               Image = x.DevOrganisation.CommonFile.FGUID,
                               MinSalary = x.MinSalary,
                               MaxSalary = x.MaxSalary,
                               Organization = new IntegerNullString() { Id = x.DevOrganisation.OId, Text = x.DevOrganisation.OrganisationName },
                               Applied = (from y in c.EmpApplyJobDetails
                                          where y.JobId == x.JobId
                                          select y.UId).Count(),
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Status = x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                               Address =  new { City = x.DevOrganisation.CommonContactAddress.City, State = x.DevOrganisation.CommonContactAddress.State } 
                           }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Jobs list get successfully!",
                    Data = res
                };
            }
        }
        /*public Result Create(object UserId, Models.Employee.Job.EE_ViewJob value)
        {
            using(DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = (from x in c.EmprJobs
                           where x.DevOrganisation.OrganisationName.Contains(value.CName) && x.SubFixedLookup_JobStatusId.FixedLookupFormatted.ToLower() == "Open"
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in c.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in c.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
                                           id = t.JobTypeId,
                                           type = t.Type
                                       }).ToList().Distinct(),
                               Image = x.DevOrganisation.CommonFile_LogoFileId.FGUID,
                               Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                               Description = x.Description,
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Organization = x.DevOrganisation.OrganisationName,
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == (int)UserId) != 0 ? "Applied" : ""),
                           }).ToList();
                if (job.Count == 0)
                {
                    throw new ArgumentException("The search job could not be found.");
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Serach Jobs Detail"),
                    Data = job
                };
            }
        }

        public Result Get(object UserId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = (from x in c.EmprJobs
                           where x.SubFixedLookup_JobStatusId.FixedLookupFormatted.ToLower() == "Open"
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in c.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in c.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
                                           id = t.JobTypeId,
                                           type = t.Type
                                       }).ToList().Distinct(),
                               Image = x.DevOrganisation.CommonFile_LogoFileId.FGUID,
                               Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                               Description = x.Description,
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Organization = x.DevOrganisation.OrganisationName,
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == (int)UserId) != 0 ? "Applied" : ""),
                           }).ToList();
                if (job.Count == 0)
                {
                    throw new ArgumentException("The search job could not be found.");
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View All Jobs"),
                    Data = job
                };
            }
        }*/
    }
}
