using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_BookMarkeds
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

                var save = (from x in c.EmpBookmarkJobsDetails
                            where x.DevOrganisation.OId == user.OId && x.JobId == Jid
                            select new
                            {
                                SaveId = x.BookMarkId,
                                UserName = x.SubUser.SubUsersDetail.FullName,
                                BranchName = x.DevOrganisationBranch.BranchName,
                                Image = x.SubUser.SubUsersDetail.CommonFile.FGUID,
                                SaveDate = x.SaveDate,
                                Status = (x.SubUser.SubUserOrganisations.Count(y => y.UId == x.UId) == 0 ? "looking for job" : "currently working"),
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"{save.Count()} Participants of {job.Title}"),
                    Data = save,
                };
            }
        }
    }
}
