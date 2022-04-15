using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_ViewJobs
    {
        public Result Create(int Uid,Models.Employee.Job.EE_ViewJob value)
        {
            using(HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var job = (from x in context.EmprJobs
                           where x.DevOrganisation.OrganisationName.Contains(value.CName)
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in context.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in context.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
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
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == Uid) != 0 ? "Applied" : ""),
                           }).ToList();
                if (job.Count == 0)
                {
                    throw new ArgumentException("The search job could not be found.");
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Serach Jobs Detail"),
                    Data = job
                };
            }
        }

        public Result Get(int Uid)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var job = (from x in context.EmprJobs
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in context.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in context.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
                                           id = t.JobTypeId,
                                           type = t.Type
                                       }).ToList().Distinct(),
                               Image = x.CommonFile.FilePath,
                               Location = x.Location,
                               Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                               Roles = x.Roles,
                               Description = x.Description,
                               PostDate = String.Format("{0:dd MMM yyyy hh:mm tt}", x.PostDate),
                               EndDate = String.Format("{0:dd MMM yyyy hh:mm tt}", x.EndDate),
                               Organization = x.DevOrganisation.OrganisationName,
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == Uid) != 0 ? "Applied" : ""),
                           }).ToList();
                if (job.Count == 0)
                {
                    throw new ArgumentException("The search job could not be found.");
                }
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View All Jobs"),
                    Data = job
                };
            }
        }
    }
}
