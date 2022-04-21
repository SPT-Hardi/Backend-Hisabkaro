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
        public Result Create(int URId,int StaffId, Models.Employer.Organization.Staff.Leave.Submit value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User not found!!");
                    }

                    var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == StaffId);
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
                        URId = staff.URId,
                        StartDate = value.StartDate,
                        EndDate = value.EndDate,
                        Reason = value.Reason,
                        IsPaidLeave = value.IsPaidLeave,
                        IsLeaveApproved = "Accepted"
                    };
                    c.OrgStaffsLeaveApplications.InsertOnSubmit(request);
                    c.SubmitChanges();
                    var name = request.SubUserOrganisation.SubUser.SubUsersDetail.FullName;
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
