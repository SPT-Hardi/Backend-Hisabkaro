using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_AppliedJobs
    {
        public Result Create(object UserId, int Jid)
        {
            var ISDT=new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var apply = (from x in c.EmpApplyJobDetails
                             orderby x.JobId descending
                             where x.OId == job.OId && x.BranchId == job.BranchID
                             && x.JobId == job.JobId && x.UId == (int)UserId
                             select x).FirstOrDefault();
                if (apply != null)
                {
                    throw new ArgumentException("Job Already Applied");
                }
                c.EmpApplyJobDetails.InsertOnSubmit(new EmpApplyJobDetail()
                {
                    UId = (int)UserId,
                    JobId = job.JobId,
                    BranchId = job.BranchID,
                    OId = (int)job.OId,
                    ApplyDate = ISDT
                });
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Apply Successfully"),
                    Data = new
                    {
                        Id = job.JobId,
                        Title = job.Title
                    }
                };
            }
        }

        public Result One(object UserId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var apply = (from x in c.EmpApplyJobDetails
                            where x.UId == (int)UserId
                             select new
                            {
                                ApplyId = x.ApplyId,
                                JobTitle = x.EmprJob.Title,
                                CompanyName = x.DevOrganisation.OrganisationName,
                                BranchName = x.DevOrganisationBranch.BranchName,
                                Type = (from y in c.EmprJobTypes
                                        where y.JobId == x.JobId
                                        select y.Type).ToList(),
                                Salary = "₹" + x.EmprJob.MinSalary + " - ₹" + x.EmprJob.MaxSalary + "/yearly"
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
