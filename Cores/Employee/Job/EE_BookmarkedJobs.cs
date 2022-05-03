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
        public Result Create(object UserId, int Jid)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                if(job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var save = (from x in c.EmpBookmarkJobsDetails
                            where x.EmprJob.DevOrganisation.OId == job.OId && x.JobId == job.JobId && x.UId == (int)UserId
                            select x).SingleOrDefault();
                if (save != null)
                {
                    save.UId = (int)UserId;
                    save.JobId = job.JobId;
                    save.OId = job.OId;
                    save.BranchId = job.BranchID == null ? null : job.BranchID;
                    save.SaveDate = ISDT;//DateTime.Now.ToLocalTime();
                    c.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Job Saved Successfully"),
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
                c.EmpBookmarkJobsDetails.InsertOnSubmit(new EmpBookmarkJobsDetail()
                {
                    UId = (int)UserId,
                    JobId = job.JobId,
                    OId = job.OId,
                    BranchId = job.BranchID == null ? null : job.BranchID,
                    SaveDate = DateTime.Now.ToLocalTime()
                });
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Saved Successfully"),
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

                var save = (from x in c.EmpBookmarkJobsDetails
                            where x.UId == (int)UserId
                            orderby x.BookMarkId descending
                            select new { 
                              SaveId = x.BookMarkId,
                              JobTitle = x.EmprJob.Title,
                              CompanyName = x.DevOrganisation.OrganisationName,
                              BranchName = x.DevOrganisationBranch.BranchName,
                              EndDate = x.EmprJob.EndDate,
                              Type = (from y in c.EmprJobTypes
                                      where y.JobId == x.JobId
                                      select y.Type).ToList(),
                              Salary = "₹" + x.EmprJob.MinSalary + " - ₹" + x.EmprJob.MaxSalary + "/yearly",
                            }).ToList();
                
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"{save.Count} Job Saved "),
                    Data = save,
                };
            }
        }

        public Result Remove(object UserId, int SaveId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = c.EmpBookmarkJobsDetails.SingleOrDefault(x => x.BookMarkId == SaveId);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var save = (from x in c.EmpBookmarkJobsDetails
                            where  x.BookMarkId == SaveId && x.UId == (int)UserId
                            select x).SingleOrDefault();
                if (save == null)
                {
                    throw new ArgumentException("Job not found!!");
                }
                c.EmpBookmarkJobsDetails.DeleteOnSubmit(save);
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Saved Job Remove Successfully"),
                    Data = new
                    {
                        Id = job.JobId,
                        Title = job.EmprJob.Title
                    }
                };
            }
        }

    }
}
