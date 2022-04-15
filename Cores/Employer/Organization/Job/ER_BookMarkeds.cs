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
        public Result Get(int Oid,int Jid)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var org = context.DevOrganisations.SingleOrDefault(x => x.OId == Oid);
                if (org == null)
                {
                    throw new ArgumentException("Org Doesn't Exist");
                }

                var job = context.EmprJobs.SingleOrDefault(x => x.JobId == Jid && x.OId == Oid );
                if (job == null)
                {
                    throw new ArgumentException("Job Doesn't Exist");
                }

                var save = (from x in context.EmpBookmarkJobsDetails
                            where x.DevOrganisation.OId == Oid && x.JobId == Jid
                            select new
                            {
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
