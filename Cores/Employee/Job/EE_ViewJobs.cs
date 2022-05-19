using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employee.Job
{
    public class EE_ViewJobs
    {
        public Result Create(object UserId, Models.Employee.Job.EE_ViewJob value)
        {
            using(DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = (from x in c.EmprJobs
                           where x.DevOrganisation.OrganisationName.Contains(value.CName) && x.Status.ToLower() == "Open"
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in c.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in c.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
                                           id = t.JobTypeId,
                                           type = t.Type
                                       }).ToList().Distinct(),
                               Image = x.DevOrganisation.CommonFile_LogoFileId.FGUID,
                               Location = x.Location,
                               Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                               Roles = x.Roles,
                               Description = x.Description,
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Organization = x.DevOrganisation.OrganisationName,
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == (int)UserId) != 0 ? "Applied" : ""),
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

        public Result Get(object UserId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUsers.SingleOrDefault(x => x.UId == (int)UserId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                var job = (from x in c.EmprJobs
                           where x.Status.ToLower() == "Open"
                           select new
                           {
                               JobID = x.JobId,
                               JobTitle = x.Title,
                               Skill = (from s in c.EmprJobSkills
                                        where s.JobId == x.JobId
                                        select new
                                        {
                                            id = s.SkillId,
                                            skill = s.Skill
                                        }).ToList().Distinct(),
                               Type = (from t in c.EmprJobTypes
                                       where t.JobId == x.JobId
                                       select new
                                       {
                                           id = t.JobTypeId,
                                           type = t.Type
                                       }).ToList().Distinct(),
                               Image = x.DevOrganisation.CommonFile_LogoFileId.FGUID,
                               Location = x.Location,
                               Salary = "₹" + x.MinSalary + " - ₹" + x.MaxSalary + "/yearly",
                               Roles = x.Roles,
                               Description = x.Description,
                               PostDate = x.PostDate,
                               EndDate = x.EndDate,
                               Organization = x.DevOrganisation.OrganisationName,
                               Status = (x.EmpApplyJobDetails.Count(y => y.UId == (int)UserId) != 0 ? "Applied" : ""),
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
