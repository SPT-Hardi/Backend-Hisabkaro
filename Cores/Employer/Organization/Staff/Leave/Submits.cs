using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Leave
{
    public class Submits
    {
        public Result Create(Models.Employer.Organization.Staff.Leave.Submit value,int UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                   // var roleid = context.SubUserOrganisations.Where(x => x.RId == value.StaffRId).SingleOrDefault();
                    OrgStaffsLeaveApplication leavesubmit = new OrgStaffsLeaveApplication();
                    leavesubmit.StartDate = value.StartDate;
                    leavesubmit.EndDate = value.EndDate;
                    leavesubmit.Reason = value.Reason;
                    leavesubmit.IsPaidLeave = value.IsPaidLeave;
                    leavesubmit.IsLeaveApproved = true;

                    context.OrgStaffsLeaveApplications.InsertOnSubmit(leavesubmit);
                    context.SubmitChanges();

                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "StaffName-{} leave details added successfully!",
                        Data = "",
                    };
                }
            }
        }
    }
}
