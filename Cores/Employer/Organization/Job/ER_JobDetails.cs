using HIsabKaro.Cores.Common.File;
using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_JobDetails
    {
        public Result Create( object URId, Models.Employer.Organization.Job.ER_JobDetail value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    if (user.SubRole.RoleName.ToLower() != "admin")
                    {
                        throw new ArgumentException("Access not allow!!");
                    }

                    //var branch = c.DevOrganisationBranches.SingleOrDefault(x => x.BranchId == (value.Branch.Id == null ? null : value.Branch.Id) && x.OId == value.Organisation.Id);
                    //if(branch == null)
                    //{
                    //    throw new ArgumentException("Branch doesn't exist");
                    //}

                    var jDetails = (from j in c.EmprJobs
                                    where j.Title == value.Title && j.Location == value.Location && j.DevOrganisation.OId == value.Organisation.Id && j.BranchID == (value.Branch.Id == null ? null : value.Branch.Id)
                                    orderby j.JobId descending
                                    select j).FirstOrDefault();

                    if (jDetails != null)
                    {
                        if (jDetails.EndDate < ISDT)
                        {
                            var _job = new EmprJob()
                            {
                                Title = value.Title,
                                Location = value.Location,
                                MinSalary = value.MinSalary,
                                MaxSalary = value.MaxSalary,
                                Roles = value.Roles,
                                Description = value.Description,
                                PostDate = ISDT,
                                EndDate = value.Enddate.ToLocalTime(),
                                OId = (int)value.Organisation.Id,
                                BranchID = value.Branch.Id == null ? null : value.Branch.Id,
                                URId = user.URId,
                                Status = "Open"
                            };
                            c.EmprJobs.InsertOnSubmit(_job);
                            c.SubmitChanges();


                            c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Select(x => new EmprJobSkill()
                            {
                                Skill = x.skill,
                                JobId = _job.JobId
                            }));
                            c.SubmitChanges();

                            c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true).Select(x => new EmprJobType()
                            {
                                Type = x.type.Text,
                                JobId = _job.JobId
                            }));
                            c.SubmitChanges();

                            return new Result()
                            {
                                Message = string.Format("Job Details Added Successfully!!"),
                                Status = Result.ResultStatus.success,
                                Data = new
                                {
                                    Id = _job.JobId,
                                    Title = _job.Title
                                },
                            };
                        }
                        else
                        {
                            throw new ArgumentException("Job Already Exist");
                        }
                    }
                    var job = new EmprJob()
                    {
                        Title = value.Title,
                        Location = value.Location,
                        MinSalary = value.MinSalary,
                        MaxSalary = value.MaxSalary,
                        Roles = value.Roles,
                        Description = value.Description,
                        PostDate = ISDT,
                        EndDate = value.Enddate.ToLocalTime(),
                        OId = (int)value.Organisation.Id,
                        BranchID = value.Branch.Id == null ? null : value.Branch.Id,
                        URId = user.URId,
                        Status = "Open"
                    };
                    c.EmprJobs.InsertOnSubmit(job);
                    c.SubmitChanges();

                    c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Select(x => new EmprJobSkill()
                    {
                        Skill = x.skill,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();

                    c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true).Select(x => new EmprJobType()
                    {
                        Type = x.type.Text,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Job Details Added Successfully!!"),
                        Data = new  { 
                              Id = job.JobId,
                              Title = job.Title,
                              Date = DateTime.Now.ToLocalTime()
                        },
                    };
                }
            }
        }

        public Result Update(object URId, int Jid,Models.Employer.Organization.Job.ER_JobDetail value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    if (user.SubRole.RoleName.ToLower() != "admin")
                    {
                        throw new ArgumentException("Access not allow!!");
                    }

                    var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid && x.OId == user.OId);
                    if (null == job)
                    {
                        throw new ArgumentException("JobDeatils doesn't exist");
                    }

                    job.Title = value.Title;
                    job.Location = value.Location;
                    job.MinSalary = value.MinSalary;
                    job.MaxSalary = value.MaxSalary;
                    job.Roles = value.Roles;
                    job.Description = value.Description;
                    job.EndDate = value.Enddate.ToLocalTime();
                    job.OId = (int)value.Organisation.Id;
                    job.BranchID = value.Branch.Id == null ? null : value.Branch.Id;
                    job.URId = user.URId;
                    job.Status = "Open";
                    c.SubmitChanges();

                    var _s = (from s in c.EmprJobSkills
                              where s.JobId == Jid
                              select s).ToList();
                    if (_s.Any())
                    {
                        c.EmprJobSkills.DeleteAllOnSubmit(_s);
                        c.SubmitChanges();
                    }

                    var _t = (from t in c.EmprJobTypes
                              where t.JobId == Jid
                              select t).ToList();
                    if (_t.Any())
                    {
                        c.EmprJobTypes.DeleteAllOnSubmit(_t);
                        c.SubmitChanges();
                    }

                    c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Select(x => new EmprJobSkill()
                    {
                        Skill = x.skill,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();

                    c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true).Select(x => new EmprJobType()
                    {
                        Type = x.type.Text,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Job Details Updated Successfully!!"),
                        Data = new
                        {
                            Id = job.JobId,
                            Title = job.Title
                        }
                    };
                }
            }
        }

        //user view specific organization job
        public Result One(object URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var query = (from x in c.EmprJobs
                             where x.OId == user.OId
                             orderby x.JobId descending
                             select new
                             {
                                 JobID = x.JobId,
                                 JobTitle = x.Title,
                                 Type = (from t in c.EmprJobTypes
                                         where t.JobId == x.JobId
                                         select new { 
                                           id = t.JobTypeId,
                                           type = t.Type
                                         }).ToList().Distinct(),
                                 Image = x.DevOrganisation.CommonFile_LogoFileId.FGUID,
                                 Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                                 Organization = x.DevOrganisation.OrganisationName,
                                 status = x.Status,
                                 Applied = (from y in c.EmpBookmarkJobsDetails
                                          where y.EmprJob.DevOrganisation.OId == user.OId && y.JobId == x.JobId 
                                          select y.UId).Count(),
                                 PostedOn = x.PostDate,
                                 EndDate = x.EndDate
                             });

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Org Job List!!"),
                    Data = query.ToList(),
                };
            }
        }

        //user view specific organization specific job
        public Result GetJob(object URId,int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                var query = (from x in c.EmprJobs
                             where x.JobId == Jid
                             orderby x.JobId descending
                             select new
                             {
                                 //id = x.JobId,
                                 //JobTitle = x.Title,
                                 //Type = (from t in c.EmprJobTypes
                                 //        where t.JobId == Jid
                                 //        select new
                                 //        {
                                 //            id = t.JobTypeId,
                                 //            type = t.Type
                                 //        }).ToList(),
                                 //Image = x.DevOrganisation.CommonFile_LogoFileId.FilePath,
                                 //Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                                 //Organization = x.DevOrganisation.OrganisationName,
                                 //PostedOn = x.PostDate.ToString("d MMMM"),
                                 Applybefore = x.EndDate,
                                 Skill = (from s in c.EmprJobSkills
                                          where s.JobId == Jid
                                          select new
                                          {
                                              id = s.SkillId,
                                              skill = s.Skill
                                          }).ToList(),
                                 Location = x.Location,
                                 Roles = x.Roles,
                                 Description = x.Description
                             }).SingleOrDefault();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View full Job Details!!"),
                    Data = query,
                };
            }
        }

        public Result RemovePost(object URId, int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                job.Status = "Remove";
                job.URId = user.URId;
                c.SubmitChanges();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Remove Successfully!!"),
                    Data = new
                    {
                        Id = job.JobId,
                        Title = job.Title
                    }
                };
            }
        }

        public Result DisablePost(object URId, int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                job.Status = "Disable";
                job.URId = user.URId;
                c.SubmitChanges();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Job Disable Successfully!!"),
                    Data = new
                    {
                        Id = job.JobId,
                        Title = job.Title
                    }
                };
            }
        }
    }
}
