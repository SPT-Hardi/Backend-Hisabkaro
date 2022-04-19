using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Job
{
    public class ER_BookMarkeds
    {
        public Result Get(int URId,int Jid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't Exist");
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
                                Image = x.SubUser.SubUsersDetail.CommonFile.FilePath,
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
