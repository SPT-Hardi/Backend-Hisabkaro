using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Leave
{
    public class Requests
    {
        public Result Create(int Uid, int Rid, Models.Employer.Organization.Staff.Leave.Request value)
        {
            using(HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var user = context.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var request = new OrgStaffsLeaveApplication()
                {
                    URId = user.URId,
                    StartDate = value.StartDate,
                    EndDate = value.EndDate,
                    Reason = value.reason,
                    IsPaidLeave = value.Ispaid
                };
                context.OrgStaffsLeaveApplications.InsertOnSubmit(request);
                context.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Apply Successfully"),
                };
            }
        }
    }
}
