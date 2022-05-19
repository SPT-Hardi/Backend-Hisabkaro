using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Leave
{
    public class Submits
    {
        public Result Create(object URId,int StaffId, Models.Employer.Organization.Staff.Leave.Submit value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == StaffId && x.OId == user.OId);
                    if (staff == null)
                    {
                        throw new ArgumentException("Staff not found!!");
                    }

                    if (user.SubRole.RoleName.ToLower() != "admin")
                    {
                        throw new ArgumentException("Access not allow!!");
                    }
                  
                    var request = new OrgStaffsLeaveApplication()
                    {
                        URId = user.URId,
                        StaffURId = staff.URId,
                        StartDate = value.StartDate.ToLocalTime(),
                        EndDate = value.EndDate.ToLocalTime(),
                        Reason = value.Reason,
                        PaidDays = (value.Paid == null ? 0 : value.Paid),
                        UnPaidDays = (value.UnPaid == null ? 0 : value.UnPaid),
                        IsLeaveApproved = "Accepted"
                    };
                    var duration = request.EndDate.Subtract(request.StartDate).Days + 1;
                    var total = request.PaidDays + request.UnPaidDays;
                    if (total > duration || total < duration)
                    {
                        throw new ArgumentException("Approve days not match with leave duration");
                    }

                    c.OrgStaffsLeaveApplications.InsertOnSubmit(request);
                    c.SubmitChanges();
                    var name = request.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName;
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff leave added successfully!",
                        Data = new {
                           Id = request.OrgStaffLeaveId,
                           Name = name
                        },
                    };
                }
            }
        }
    }
}
