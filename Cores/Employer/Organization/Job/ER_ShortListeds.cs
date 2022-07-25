using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HIsabKaro.Cores.Employer.Organization.Job.ER_JobDetails;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_ShortListeds
    {
        public Result Add(object URId,int UId,int JId) 
        {
            using (DBContext c = new DBContext())
            {
                if (URId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var u = c.SubUsers.FirstOrDefault(x => x.UId == UId);
                if (u == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }
                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == JId && o.SubUserOrganisation.UId==user.UId && o.JobStatusId != (int)JobStatus.Remove);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }
                c.EmprApplicantShortListDetails.InsertOnSubmit(new EmprApplicantShortListDetail()
                {
                    ApplicantUId=UId,
                    BranchId=job.BranchID,
                    JobId=job.JobId,
                    MarkedByURId=(int)URId,
                    MarkedDate=DateTime.Now,
                    OId=job.OId,
                });
                c.SubmitChanges();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Applicant shortlisted successfully!",
                    Data = new 
                    {
                        Shorlisted_UserName=u.SubUsersDetail.FullName,
                        Shorlisted_UId=u.UId,
                    }
                };
            }
        }
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

                var applicants = (from x in c.EmprApplicantShortListDetails
                                  where x.JobId == Jid
                                  select new Models.Employer.Organization.Job.Applicants()
                                  {
                                      Id = x.ShortListId,
                                      Date = x.MarkedDate,
                                      IsApplied = (from y in c.EmpApplyJobDetails where x.ApplicantUId == y.UId && x.JobId == y.JobId select x).Any() ? true : false,
                                      ImageFGUID = x.SubUser.SubUsersDetail.CommonFile.FGUID,
                                      IsBookmarked= (from y in c.EmpBookmarkJobsDetails where x.ApplicantUId == y.UId && x.JobId == y.JobId select x).Any() ? true : false,
                                      IsShortListed = true,
                                      MobileNumber = x.SubUser.MobileNumber,
                                      Name = x.SubUser.SubUsersDetail.FullName,
                                      skills = x.SubUser.EmpResumeSkills.ToList().Select(y => new Models.Employer.Organization.Job.JobSkill() { skill = y.SkillName }).ToList(),
                                      UId = x.ApplicantUId,
                                      //WorkExperience = new Employee.Resume.WorkExperiences().TotalExperience(x.ApplicantUId),
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
