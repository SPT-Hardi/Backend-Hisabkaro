using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.File;
using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_JobDetails
    {
        public enum JobStatus
        {
            Open = 52,
            Disable = 53,
            Remove = 54
        }
/*        public Result Org_and_BranchDrop(object UId)
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var organizationlist = (from x in c.DevOrganisations where x.UId == (int)UId select x).ToList();
                foreach()
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "",
                    Data = ""
                };
            }
        }*/
        public Result Create(object URId, Models.Employer.Organization.Job.ER_JobDetail value)
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

                    if (ISDT > value.EndDate)
                    {
                        throw new ArgumentException("Post Date Can't be After End Date.");
                    }

                    var jDetails = (from j in c.EmprJobs
                                    where j.Title == value.Title  && j.DevOrganisation.OId == value.Organisation.Id 
                                    orderby j.JobId descending
                                    select j).FirstOrDefault();

                    if (jDetails != null)
                    {
                        if (jDetails.EndDate > ISDT)
                        {
                            throw new ArgumentException("Job Already Exist");
                        }
                    }

                    var _job = new EmprJob()
                    {
                        Title = value.Title,
                        MinSalary = value.MinSalary,
                        MaxSalary = value.MaxSalary,
                        Description = value.Description,
                        PostDate = ISDT,
                        EndDate = value.EndDate.ToLocalTime(),
                        OId = (int)value.Organisation.Id,
                        URId = user.URId,
                        JobStatusId = (int)JobStatus.Open,
                        AddressId=value.AddressId,
                        Comment=value.Comment,
                        Email=value.Email,
                        IncentiveTypeId=value.IncentiveType.Id==0? null : value.IncentiveType.Id,
                        JobStartTime=value.JobStartTime,
                        JobEndTime=value.JobEndTime,
                        MaxIncentive=value.MaxIncentive,
                        MinIncentive=value.MinIncentive,
                        MobileNumber=value.MobileNumber,
                        SalaryTypeId=value.SalaryType.Id==0 ? null : value.SalaryType.Id,
                        LastUpdated=DateTime.Now,
                        IsUpdated=false,
                    };
                    c.EmprJobs.InsertOnSubmit(_job);
                    c.SubmitChanges();

                    c.EmprJobEnglishLevels.InsertAllOnSubmit(value.EnglishLevels.Where(x => x.level != null).Select(x => new EmprJobEnglishLevel()
                    {
                        JobId = _job.JobId,
                        EnglishLevel = x.level,
                    }));
                    c.SubmitChanges();

                    c.EmprJobOtherLanguages.InsertAllOnSubmit(value.OtherLanguages.Where(x => x.language != null).Select(x => new EmprJobOtherLanguage()
                    {
                        JobId = _job.JobId,
                        OtherLanguage=x.language,
                    }));
                    c.SubmitChanges();

                    c.EmprJobExperienceLevels.InsertAllOnSubmit(value.ExperienceLevels.Where(x=>x.level!=null).Select(x => new EmprJobExperienceLevel()
                    {
                        JobId= _job.JobId,
                        ExperienceLevel=x.level,
                    }));
                    c.SubmitChanges();

                    c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Where(x=>x.skill!=null).Select(x => new EmprJobSkill()
                    {
                        Skill = x.skill,
                        JobId = _job.JobId
                    }));
                    c.SubmitChanges();

                    c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true && x.type!=null).Select(x => new EmprJobType()
                    {
                        Type = x.type,
                        JobId = _job.JobId
                    }));
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Job Details Added Successfully!!"),
                        Data = new
                        {
                            Id = _job.JobId,
                            Title = _job.Title
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
                        throw new ArgumentException("User not found!!");

                    if(user.OId != value.Organisation.Id)
                        throw new ArgumentException("not authorized!");

                    if (user.SubRole.RoleName.ToLower() != "admin")
                        throw new ArgumentException("Access not allow!!");
                    
                    if (ISDT > value.EndDate)
                        throw new ArgumentException("Post date can't be after end date.");
                    
                    var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid && x.JobStatusId!=(int)JobStatus.Remove && x.EndDate<DateTime.Now);

                    if (null == job)
                        throw new ArgumentException("JobDeatils doesn't exist");
                    if (job.OId != user.OId && job.OId != value.Organisation.Id)
                        throw new ArgumentException("not authorized!!"); 


                    job.Title = value.Title;
                    job.MinSalary = value.MinSalary;
                    job.MaxSalary = value.MaxSalary;
                    job.Description = value.Description;
                    job.EndDate = value.EndDate.ToLocalTime();
                    job.OId = (int)value.Organisation.Id;
                    job.URId = user.URId;
                    job.JobStatusId = (int)JobStatus.Open;
                    job.AddressId = value.AddressId;
                    job.Comment = value.Comment;
                    job.Email = value.Email;
                    job.IncentiveTypeId = value.IncentiveType.Id==0 ? null : value.IncentiveType.Id;
                    job.JobStartTime = value.JobStartTime;
                    job.JobEndTime = value.JobEndTime;
                    job.MaxIncentive = value.MaxIncentive;
                    job.MinIncentive = value.MinIncentive;
                    job.MobileNumber = value.MobileNumber;
                    job.SalaryTypeId = value.SalaryType.Id==0? null : value.SalaryType.Id;
                    job.IsUpdated = true;
                    job.LastUpdated = DateTime.Now;
                /*    job.Title = value.Title;
                    job.Location = value.Location;
                    job.MinSalary = value.MinSalary;
                    job.MaxSalary = value.MaxSalary;
                    job.Roles = value.Roles;
                    job.Description = value.Description;
                    job.EndDate = value.Enddate.ToLocalTime();
                    job.OId = (int)value.Organisation.Id;
                    job.BranchID = value.Branch.Id == null ? null : value.Branch.Id;
                    job.URId = user.URId;
                    job.JobStatusId = (int)JobStatus.Open;*/
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

                    var _exp = (from t in c.EmprJobExperienceLevels
                              where t.JobId == Jid
                              select t).ToList();
                    if (_exp.Any())
                    {
                        c.EmprJobExperienceLevels.DeleteAllOnSubmit(_exp);
                        c.SubmitChanges();
                    }

                    var _english = (from t in c.EmprJobEnglishLevels
                                where t.JobId == Jid
                                select t).ToList();
                    if (_english.Any())
                    {
                        c.EmprJobEnglishLevels.DeleteAllOnSubmit(_english);
                        c.SubmitChanges();
                    }

                    var _language = (from t in c.EmprJobOtherLanguages
                                    where t.JobId == Jid
                                    select t).ToList();
                    if (_language.Any())
                    {
                        c.EmprJobOtherLanguages.DeleteAllOnSubmit(_language);
                        c.SubmitChanges();
                    }

                    c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Where(x=>x.skill!=null).Select(x => new EmprJobSkill()
                    {
                        Skill = x.skill,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();

                    c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true && x.type!=null).Select(x => new EmprJobType()
                    {
                        Type = x.type,
                        JobId = job.JobId
                    }));
                    c.SubmitChanges();

                    c.EmprJobExperienceLevels.InsertAllOnSubmit(value.ExperienceLevels.Where(x=>x.level!=null).Select(x => new EmprJobExperienceLevel()
                    {
                        JobId = job.JobId,
                        ExperienceLevel = x.level,
                    }));
                    c.SubmitChanges();

                    c.EmprJobEnglishLevels.InsertAllOnSubmit(value.EnglishLevels.Where(x => x.level != null).Select(x => new EmprJobEnglishLevel()
                    {
                        JobId = job.JobId,
                        EnglishLevel = x.level,
                    }));
                    c.SubmitChanges();

                    c.EmprJobOtherLanguages.InsertAllOnSubmit(value.OtherLanguages.Where(x => x.language != null).Select(x => new EmprJobOtherLanguage()
                    {
                        JobId = job.JobId,
                        OtherLanguage = x.language,
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

        //user view specific organization job with less Description
        public Result One(object URId)
        {
            using (DBContext c = new DBContext())
            {
                if (URId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }
                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var res = (from x in c.EmprJobs
                             where x.OId==user.OId 
                             orderby x.JobId descending
                             select new
                             {
                                 JobId = x.JobId,
                                 JobTitle = x.Title,
                                 JobType = (from t in c.EmprJobTypes
                                         where t.JobId == x.JobId
                                         select new {Type = t.Type}).ToList(),
                                 Image = x.DevOrganisation.CommonFile.FGUID,
                                 MinSalary = x.MinSalary,
                                 MaxSalary = x.MaxSalary,
                                 Organization = new IntegerNullString() { Id = x.DevOrganisation.OId, Text = x.DevOrganisation.OrganisationName },
                                 Applied = (from y in c.EmpApplyJobDetails
                                            where y.JobId == x.JobId
                                            select y.UId).Count(),
                                 PostDate = x.PostDate,
                                 EndDate = x.EndDate,
                                 Status = x.EndDate<DateTime.Now==true ? "Removed" :x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                                 Address= new {City=x.DevOrganisation.CommonContactAddress.City,State= x.DevOrganisation.CommonContactAddress.State }  
                             }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Org Job List!!"),
                    Data = res,
                };
            }
        }

        //user view specific organization specific job
        public Result GetJob(object URId, int Jid)
        {
            using (DBContext c = new DBContext())
            {
                if (URId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId && o.JobStatusId != (int)JobStatus.Remove);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                var res = (from x in c.EmprJobs
                           where x.JobId == Jid 
                           orderby x.JobId descending
                           select new
                           {
                               JobId = x.JobId,
                               SalaryType = new IntegerNullString() { Id = x.SubFixedLookup_SalaryTypeId.FixedLookupId, Text = x.SubFixedLookup_SalaryTypeId.FixedLookup, },
                               AddressId = x.AddressId,
                               Address=new Models.Common.Contact.Address() 
                               {
                                   AddressId=x.AddressId,
                                   AddressLine1=x.CommonContactAddress.AddressLine1,
                                   AddressLine2="",
                                   City=x.CommonContactAddress.City,
                                   LandMark=x.CommonContactAddress.Landmark,
                                   PinCode=x.CommonContactAddress.PinCode,
                                   State=x.CommonContactAddress.State
                               },
                               Comment = x.Comment,
                               Description = x.Description,
                               Email = x.Email,
                               EndDate = x.EndDate,
                               EnglishLevel = x.EmprJobEnglishLevels.ToList().Select(z=> new {level=z.EnglishLevel }).ToList(),
                               OtherLanguage = x.EmprJobOtherLanguages.ToList().Select(z => new { language = z.OtherLanguage }).ToList(),
                               ExperienceLevels =x.EmprJobExperienceLevels.ToList().Select(z => new { level = z.ExperienceLevel }).ToList(),
                               IncentiveType =new IntegerNullString() { Id=x.SubFixedLookup_IncentiveTypeId.FixedLookupId,Text=x.SubFixedLookup_IncentiveTypeId.FixedLookup,},
                               JobEndDate =x.EndDate,
                               jobSkill =x.EmprJobSkills.ToList().Select(z=>new { skill=z.Skill}).ToList(),
                               JobStartTime =x.JobStartTime,
                               jobType =x.EmprJobTypes.ToList().Select(z=>new { type=z.Type}).ToList(),
                               MaxIncentive =x.MaxIncentive,
                               MaxSalary =x.MaxSalary,
                               MinIncentive =x.MinIncentive,
                               MinSalary =x.MinSalary,
                               MobileNumber =x.MobileNumber,
                               Status= x.EndDate < DateTime.Now == true ? "Removed" :x.SubFixedLookup_JobStatusId.FixedLookupFormatted,
                               Organization =new IntegerNullString() { Id=x.DevOrganisation.OId,Text=x.DevOrganisation.OrganisationName,},
                               Title =x.Title,
                               Applied = (from y in c.EmpApplyJobDetails
                                          where y.JobId == x.JobId
                                          select y.UId).Count(),
                               Bookmarked = (from y in c.EmpBookmarkJobsDetails
                                             where y.JobId == x.JobId
                                             select y.UId).Count(),
                               ShortListed = (from y in c.EmprApplicantShortListDetails
                                              where y.JobId == x.JobId
                                              select y.ApplicantUId).Count(),
                           }).FirstOrDefault();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View full Job Details!!"),
                    Data = res,
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

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId && o.JobStatusId != (int)JobStatus.Remove && o.EndDate < DateTime.Now);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                job.JobStatusId = (int)JobStatus.Remove;
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

        public Result Disable_Enable(object URId, int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var job = c.EmprJobs.SingleOrDefault(o => o.JobId == Jid && o.OId == user.OId && o.JobStatusId != (int)JobStatus.Remove && o.EndDate < DateTime.Now);
                if (job == null)
                {
                    throw new ArgumentException("Job Not Found!!");
                }

                job.JobStatusId =job.JobStatusId==(int)JobStatus.Disable ? (int)JobStatus.Open : (int)JobStatus.Disable;
                job.URId = user.URId;
                c.SubmitChanges();
                var status = job.SubFixedLookup_JobStatusId.FixedLookup;
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"Job {status} Successfully!!"),
                    Data = new
                    {
                        Id = job.JobId,
                        Title = job.Title
                    }
                };
            }
        }
        public Result JobTypeSearch(string keyword)
        {
            using (DBContext c = new DBContext()) 
            {
                var res = (from x in c.SubFixedLookups
                           where x.FixedLookupType == "JobType/Role" && x.FixedLookup.Contains(keyword)
                           orderby x.FixedLookup.IndexOf(keyword) ascending
                           select new IntegerNullString()
                           {
                               Id = x.FixedLookupId,
                               Text = x.FixedLookup.Trim(),
                           }).ToList();
            return new Result()
            {
                Status=Result.ResultStatus.success,
                Message="JobType search list geted successfully!",
                Data=res,
            };
            }
        }
  
        //public Result Create( object URId, Models.Employer.Organization.Job.ER_JobDetail value)
        //{
        //    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
        //            if (user == null)
        //            {
        //                throw new ArgumentException("User not found!!");
        //            }

        //            if (user.SubRole.RoleName.ToLower() != "admin")
        //            {
        //                throw new ArgumentException("Access not allow!!");
        //            }

        //            if (ISDT > value.Enddate)
        //            {
        //                throw new ArgumentException("Post Date Can't be After End Date.");
        //            }

        //            //var branch = c.DevOrganisationBranches.SingleOrDefault(x => x.BranchId == (value.Branch.Id == null ? null : value.Branch.Id) && x.OId == value.Organisation.Id);
        //            //if(branch == null)
        //            //{
        //            //    throw new ArgumentException("Branch doesn't exist");
        //            //}

        //            var jDetails = (from j in c.EmprJobs
        //                            where j.Title == value.Title && j.Location == value.Location && j.DevOrganisation.OId == value.Organisation.Id && j.BranchID == (value.Branch.Id == null ? null : value.Branch.Id)
        //                            orderby j.JobId descending
        //                            select j).FirstOrDefault();

        //            if (jDetails != null)
        //            {
        //                if (jDetails.EndDate < ISDT)
        //                {
        //                    var _job = new EmprJob()
        //                    {
        //                        Title = value.Title,
        //                        Location = value.Location,
        //                        MinSalary = value.MinSalary,
        //                        MaxSalary = value.MaxSalary,
        //                        Roles = value.Roles,
        //                        Description = value.Description,
        //                        PostDate = ISDT,
        //                        EndDate = value.Enddate.ToLocalTime(),
        //                        OId = (int)value.Organisation.Id,
        //                        BranchID = value.Branch.Id == null ? null : value.Branch.Id,
        //                        URId = user.URId,
        //                        Status = "Open"
        //                    };
        //                    c.EmprJobs.InsertOnSubmit(_job);
        //                    c.SubmitChanges();


        //                    c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Select(x => new EmprJobSkill()
        //                    {
        //                        Skill = x.skill,
        //                        JobId = _job.JobId
        //                    }));
        //                    c.SubmitChanges();

        //                    c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true).Select(x => new EmprJobType()
        //                    {
        //                        Type = x.type.Text,
        //                        JobId = _job.JobId
        //                    }));
        //                    c.SubmitChanges();

        //                    return new Result()
        //                    {
        //                        Message = string.Format("Job Details Added Successfully!!"),
        //                        Status = Result.ResultStatus.success,
        //                        Data = new
        //                        {
        //                            Id = _job.JobId,
        //                            Title = _job.Title
        //                        },
        //                    };
        //                }
        //                else
        //                {
        //                    throw new ArgumentException("Job Already Exist");
        //                }
        //            }
        //            var job = new EmprJob()
        //            {
        //                Title = value.Title,
        //                Location = value.Location,
        //                MinSalary = value.MinSalary,
        //                MaxSalary = value.MaxSalary,
        //                Roles = value.Roles,
        //                Description = value.Description,
        //                PostDate = ISDT,
        //                EndDate = value.Enddate.ToLocalTime(),
        //                OId = (int)value.Organisation.Id,
        //                BranchID = value.Branch.Id == null ? null : value.Branch.Id,
        //                URId = user.URId,
        //                Status = "Open"
        //            };
        //            c.EmprJobs.InsertOnSubmit(job);
        //            c.SubmitChanges();

        //            c.EmprJobSkills.InsertAllOnSubmit(value.jobSkill.Select(x => new EmprJobSkill()
        //            {
        //                Skill = x.skill,
        //                JobId = job.JobId
        //            }));
        //            c.SubmitChanges();

        //            c.EmprJobTypes.InsertAllOnSubmit(value.jobType.Where(x => x.status == true).Select(x => new EmprJobType()
        //            {
        //                Type = x.type.Text,
        //                JobId = job.JobId
        //            }));
        //            c.SubmitChanges();
        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format("Job Details Added Successfully!!"),
        //                Data = new  { 
        //                      Id = job.JobId,
        //                      Title = job.Title
        //                },
        //            };
        //        }
        //    }
        //}

    }
}
