using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HIsabKaro.Cores.Employer.Organization.Job.ER_JobDetails;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_AppliedJobs
    {
        public Result Get(object URId, int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.SubUserOrganisation.UId == user.UId && o.JobStatusId != (int)JobStatus.Remove);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var applicants = (from x in c.EmpApplyJobDetails
                                  where x.JobId == Jid
                                  select new Models.Employer.Organization.Job.Applicants()
                                  {
                                      Id = x.ApplyId,
                                      Date = x.ApplyDate,
                                      IsApplied = true,
                                      ImageFGUID = x.SubUser.SubUsersDetail.CommonFile.FGUID,
                                      IsBookmarked = (from y in c.EmpBookmarkJobsDetails where x.UId == y.UId && x.OId == y.OId select x).Any() ? true : false,
                                      IsShortListed = (from y in c.EmprApplicantShortListDetails where x.UId == y.ApplicantUId && x.OId == y.OId select x).Any() ? true : false,
                                      MobileNumber = x.SubUser.MobileNumber,
                                      Name = x.SubUser.SubUsersDetail.FullName,
                                      skills = x.SubUser.EmpResumeSkills.ToList().Select(y => new Models.Employer.Organization.Job.JobSkill() { skill = y.SkillName }).ToList(),
                                      UId = x.UId,
                                      WorkExperience = new Employee.Resume.WorkExperiences().TotalExperience(x.UId),
                                  }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Users list get successfully,who applied for job!",
                    Data = new Models.Employer.Organization.Job.Applied_Bookmarked_ShortListed_List()
                    {
                        Applicants = applicants,
                        Total = applicants.Count(),
                    },
                };
            }
        }
    }
}
