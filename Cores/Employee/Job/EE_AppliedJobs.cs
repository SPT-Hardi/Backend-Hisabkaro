using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_AppliedJobs
    {
        public Result Create(int Uid, int Jid)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var job = context.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var apply = (from x in context.EmpApplyJobDetails
                             orderby x.JobId descending
                             where x.OId == job.OId && x.BranchId == job.BranchID
                             && x.JobId == job.JobId && x.UId == Uid
                            select x).FirstOrDefault();
                if (apply != null)
                {
                    throw new ArgumentException("Job Already Applied");
                }
                context.EmpApplyJobDetails.InsertOnSubmit(new EmpApplyJobDetail()
                {
                    UId = Uid,
                    JobId = job.JobId,
                    BranchId = (int)job.BranchID,
                    OId = (int)job.OId,
                    ApplyDate = DateTime.Now
                });
                context.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Apply Successfully"),
                };
            }
        }

        public Result One(int Uid)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var user = context.SubUsers.SingleOrDefault(x => x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var apply = (from x in context.EmpApplyJobDetails
                            where x.UId == Uid
                            select new
                            {
                                JobTitle = x.EmprJob.Title,
                                CompanyName = x.DevOrganisation.OrganisationName,
                                BranchName = x.DevOrganisationBranch.BranchName,
                                EndDate = String.Format("{0:dd MMM yyyy hh:mm tt}", x.EmprJob.EndDate),
                                ApplyDate = String.Format("{0:dd MMM yyyy hh:mm tt}", x.ApplyDate),
                                Location = x.EmprJob.Location
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"{apply.Count} Apply Job"),
                    Data = apply,
                };
            }
        }
    }
}
