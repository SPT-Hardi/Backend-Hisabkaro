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
        public Result Get(int Jid,int Uid,int Rid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.UId == Uid && x.RId == Rid);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
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
                                UserName = x.SubUser.SubUsersDetail.FullName,
                                Experience = x.SubUser.SubUsersTotalworkexperiences.Where(y => y.UId == x.UId).Select(y => y.Duration),
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
