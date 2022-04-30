using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_AppliedJobs
    {
        public Result Get(object URId,int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                } 

                var job = c.EmprJobs.SingleOrDefault(x => x.JobId == Jid && x.OId == user.OId);
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var apply = (from x in c.EmpApplyJobDetails
                            where x.DevOrganisation.OId == user.OId && x.JobId == Jid
                            select new
                            {
                                ApplyId = x.ApplyId,
                                UserName = x.SubUser.SubUsersDetail.FullName,
                                Experience = (from y in c.SubUsersTotalworkexperiences
                                              where y.UId == x.SubUser.UId
                                              select y.Duration).SingleOrDefault(),
                                Image = x.SubUser.SubUsersDetail.CommonFile.FilePath,
                                ApplyDate = x.ApplyDate,
                                Status = (x.SubUser.SubUserOrganisations.Count(y => y.UId == x.UId) == 0 ? "looking for job" : "currently working"),
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"{apply.Count()} Participants of {job.Title}"),
                    Data = apply,
                };
            }
        }
    }
}
