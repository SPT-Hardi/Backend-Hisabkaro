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
        public Result Create(int URId,int Uid,int Rid,Models.Employer.Organization.Staff.Leave.Submit value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (staff == null)
                    {
                        throw new ArgumentException("Staff not found!!");
                    }

                    var request = new OrgStaffsLeaveApplication()
                    {
                        URId = URId,
                        StartDate = value.StartDate,
                        EndDate = value.EndDate,
                        Reason = value.Reason,
                        IsPaidLeave = value.IsPaidLeave,
                        IsLeaveApproved = true
                    };
                    c.OrgStaffsLeaveApplications.InsertOnSubmit(request);
                    c.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff leave details added successfully!",
                        Data = new {
                           Id = request.OrgStaffLeaveId,
                        },
                    };
                }
            }
        }
    }
}
