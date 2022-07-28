using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static HIsabKaro.Cores.Employer.Organization.Job.ER_JobDetails;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_BookmarkedJobs
    {
        public Result BookMarkedJob_Toggle(object UId,int Jid) 
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
                var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid && x.JobStatusId!=(int)JobStatus.Remove && x.JobStatusId != (int)JobStatus.Disable && x.EndDate<DateTime.Now);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }
                var empjob = job.EmpBookmarkJobsDetails.ToList().Where(z => z.UId == user.UId).FirstOrDefault();
                if (empjob != null)
                {
                    c.EmpBookmarkJobsDetails.DeleteOnSubmit(empjob);
                    c.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Bookmarke removed successfully!",
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
                else
                {
                    var j = new EmpBookmarkJobsDetail()
                    {
                        UId = user.UId,
                        JobId = job.JobId,
                        SaveDate=DateTime.Now,
                    };
                    c.EmpBookmarkJobsDetails.InsertOnSubmit(j);
                    c.SubmitChanges();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Bookmarked for job successfully!",
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
            }
        }
        public Result BookmarkedJob_List(object UId) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var res = (from x in c.EmprJobs
                           where /*x.JobStatusId != (int)JobStatus.Disable && x.JobStatusId != (int)JobStatus.Remove && */(from c in c.EmpBookmarkJobsDetails where c.UId==(int)UId && c.JobId==x.JobId select x).Any() ? true : false 
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
                               Status = x.EndDate<DateTime.Now==true ? "Remove" : x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
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
       /* public Result Create(object UserId, int Jid)
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
*/
    }
}
