using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Leave
{
    public class Approves
    {
        public Result Get(int Uid, int Rid)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var leave = (from x in c.OrgStaffsLeaveApplications
                             where x.SubUserOrganisation.OId == user.OId
                             select new
                             {
                                 Id = x.OrgStaffLeaveId,
                                 UserName = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                 Leave = "From :" + x.StartDate + " To :" + x.EndDate
                             }).ToList();
                return new Result()
                {
                    Message = string.Format("Leave Request"),
                    Status = Result.ResultStatus.success,
                    Data = leave,
                };
            }
        }

        public Result Update(int Uid, int Rid, int leaveId, Models.Employer.Organization.Staff.Leave.Approve value)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.RId == Rid && x.UId == Uid);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var leave = c.OrgStaffsLeaveApplications.SingleOrDefault(x => x.OrgStaffLeaveId == leaveId
                                                                          && x.SubUserOrganisation.OId == user.OId);
                if (leave == null)
                {
                    throw new ArgumentException("Data not found!!");
                }

                leave.IsLeaveApproved = true;
                leave.IsPaidLeave = value.Ispaid;
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Approve Successfully"),
                    Data = new
                    {
                        Id = leave.OrgStaffLeaveId
                    }
                };
            }
        }
    }
}
