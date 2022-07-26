using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HIsabKaro.Cores.Employer.Organization.Job.ER_JobDetails;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_AppliedJobs
    {
        public Result Applied_Toggle(object UId,int Jid)
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
                var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }
                var empjob =job.EmpApplyJobDetails.ToList().Where(z => z.UId == user.UId).FirstOrDefault();
                if (empjob != null)
                {
                    c.EmpApplyJobDetails.DeleteOnSubmit(empjob);
                    c.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Job application revoked successfully!",
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
                else 
                {
                    var j = new EmpApplyJobDetail()
                    {
                        ApplyDate=DateTime.Now,
                        UId=user.UId,
                        JobId=job.JobId,
                    };
                    c.EmpApplyJobDetails.InsertOnSubmit(j);
                    c.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Applied for job successfully!",
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
            }
        }
        public Result Applied_List(object UId) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var res = (from x in c.EmprJobs
                           where x.JobStatusId != (int)JobStatus.Disable && x.JobStatusId != (int)JobStatus.Remove && (from c in c.EmpApplyJobDetails where c.UId==(int)UId && c.JobId==x.JobId select x).Any() ? true : false
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
                    Message = "List of applied jobs get successfully!",
                    Data = res
                };
            }
        }
    }
}
