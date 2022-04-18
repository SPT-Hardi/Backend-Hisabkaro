﻿using HIsabKaro.Cores.Common.File;
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
        private readonly Uploads _images;

        public ER_JobDetails(Uploads images)
        {
            _images = images;
        }

        public Result Create( int Uid,int Rid,Models.Employer.Organization.Job.ER_JobDetail value)
        {
            using(DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    var jDetails = (from j in c.EmprJobs
                                    where j.Title == value.Title && j.Location == value.Location && j.DevOrganisation.OId == value.Organisation.ID
                                    orderby j.JobId descending
                                    select j).FirstOrDefault();
                   
                    if (jDetails != null)
                    {
                        if (jDetails.EndDate < DateTime.Now)
                        {
                            var _job = new EmprJob()
                            {
                                Title = value.Title,
                                FileId = value.Imageid,
                                Location = value.Location,
                                MinSalary = value.MinSalary,
                                MaxSalary = value.MaxSalary,
                                Roles = value.Roles,
                                Description = value.Description,
                                PostDate = DateTime.Now,
                                EndDate = value.Enddate.ToLocalTime(),
                                OId = (int)value.Organisation.ID,
                                BranchID = value.Branch.ID == null ? null : value.Branch.ID,
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
                        FileId = value.Imageid,
                        Location = value.Location,
                        MinSalary = value.MinSalary,
                        MaxSalary = value.MaxSalary,
                        Roles = value.Roles,
                        Description = value.Description,
                        PostDate = DateTime.Now,
                        EndDate = value.Enddate.ToLocalTime(),
                        OId = (int)value.Organisation.ID,
                        BranchID = value.Branch.ID == null ? null : value.Branch.ID,
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
                              Title = job.Title
                        },
                    };
                }
            }
        }

        public Result Update(int Uid,int Rid,int Jid,Models.Employer.Organization.Job.ER_JobDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid);
                    if (null == job)
                    {
                        throw new ArgumentException("JobDeatils doesn't exist");
                    }

                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    job.Title = value.Title;
                    job.FileId = value.Imageid;
                    job.Location = value.Location;
                    job.MinSalary = value.MinSalary;
                    job.MaxSalary = value.MaxSalary;
                    job.Roles = value.Roles;
                    job.Description = value.Description;
                    job.PostDate = DateTime.Now;
                    job.EndDate = value.Enddate.ToLocalTime();
                    job.OId = (int)value.Organisation.ID;
                    job.BranchID = value.Branch.ID;
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
        public Result One(int Uid,int Rid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var query = (from x in c.EmprJobs
                             where x.OId == user.OId
                             orderby x.JobId descending
                             select new
                             {
                                 JobID = x.JobId,
                                 JobTitle = x.Title,
                                 Skill = (from s in c.EmprJobSkills
                                          where s.JobId == x.JobId
                                          select new { 
                                              id = s.SkillId,
                                              skill = s.Skill
                                          }).ToList().Distinct(),
                                 Type = (from t in c.EmprJobTypes
                                         where t.JobId == x.JobId
                                         select new { 
                                           id = t.JobTypeId,
                                           type = t.Type
                                         }).ToList().Distinct(),
                                 Image = x.CommonFile.FilePath,
                                 Location = x.Location,
                                 Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                                 Roles = x.Roles,
                                 Description = x.Description,
                                 PostDate = x.PostDate,
                                 EndDate = x.EndDate,
                                 Organization = x.DevOrganisation.OrganisationName,
                                 count = (from y in c.EmpBookmarkJobsDetails
                                          where y.EmprJob.DevOrganisation.OId == user.OId && y.JobId == x.JobId
                                          select y.UId).Count(),
                             });

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View Org all Job!!"),
                    Data = query.ToList(),
                };
            }
        }

        //user view specific organization specific job
        public Result GetJob(int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid);

                if (job == null)
                {
                    throw new ArgumentException("No Job Found");
                }

                var query = (from x in c.EmprJobs
                             where x.JobId == Jid
                             orderby x.JobId descending
                             select new
                             {
                                 id = x.JobId,
                                 JobTitle = x.Title,
                                 Skill = (from s in c.EmprJobSkills
                                          where s.JobId == Jid
                                          select new
                                          {
                                              id = s.SkillId,
                                              skill = s.Skill
                                          }).ToList(),
                                 Type = (from t in c.EmprJobTypes
                                         where t.JobId == Jid
                                         select new
                                         {
                                             id = t.JobTypeId,
                                             type = t.Type
                                         }).ToList(),
                                 Image = x.CommonFile.FilePath,
                                 Location = x.Location,
                                 Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                                 Roles = x.Roles,
                                 Description = x.Description,
                                 PostDate = x.PostDate,
                                 EndDate = x.EndDate,
                                 Organization = x.DevOrganisation.OrganisationName
                             }).SingleOrDefault();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View full Job Details!!"),
                    Data = query,
                };
            }
        }

        public Result RemovePost(int Uid,int Rid,int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid);

                if (job == null)
                {
                    throw new ArgumentException("No Job Found");
                }

                var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
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

        public Result DisablePost(int Uid,int Rid,int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid);

                if (job == null)
                {
                    throw new ArgumentException("No Job Found");
                }

                var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
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