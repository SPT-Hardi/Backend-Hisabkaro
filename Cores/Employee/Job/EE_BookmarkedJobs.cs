using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_BookmarkedJobs
    {
        public Result Create(int Uid,int Jid)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var job = context.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                if(job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var save = (from x in context.EmpBookmarkJobsDetails
                            where x.EmprJob.DevOrganisation.OId == job.OId && x.JobId == job.JobId && x.UId == Uid
                            select x).SingleOrDefault();
                if (save != null)
                {
                    save.UId = Uid;
                    save.JobId = job.JobId;
                    save.OId = job.OId;
                    save.BranchId = job.BranchID;
                    save.SaveDate = DateTime.Now;
                    context.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Job Saved Successfully"),
                    };
                }
                context.EmpBookmarkJobsDetails.InsertOnSubmit(new EmpBookmarkJobsDetail()
                {
                    UId = Uid,
                    JobId = job.JobId,
                    OId = job.OId,
                    BranchId = job.BranchID,
                    SaveDate = DateTime.Now
                });
                context.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Saved Successfully"),
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

                var save = (from x in context.EmpBookmarkJobsDetails
                            where x.UId == Uid
                            select new { 
                              JobTitle = x.EmprJob.Title,
                              CompanyName = x.DevOrganisation.OrganisationName,
                              BranchName = x.DevOrganisationBranch.BranchName,
                              EndDate = String.Format("{0:dd,MMM}", x.EmprJob.EndDate),
                              //String.Format("{0:dd MMM yyyy hh:mm tt}", x.EmprJob.EndDate),
                              //SaveDate = String.Format("{0:dd MMM yyyy hh:mm tt}", x.SaveDate),
                              //Location = x.EmprJob.Location,
                                //count = (from y in context.EmprJobs
                                //         where y.UId == Uid
                                //         select y.JobId).Count(),

                            }).ToList();
                
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"{save.Count} Job Saved "),
                    Data = save,
                };
            }
        }

    }
}
