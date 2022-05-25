using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Leave
{
    public class Approves
    {
        public enum LeaveStatus
        {
            Pending = 55,
            Accepted = 56,
            Reject = 57
        }

        public Result Get(object URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if(user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var leave = (from x in c.OrgStaffsLeaveApplications
                             where x.SubUserOrganisation_StaffURId.OId == user.OId && x.SubFixedLookup.FixedLookupFormatted == "Pending"
                             orderby x.OrgStaffLeaveId descending
                             select new
                             {
                                 Id = x.OrgStaffLeaveId,
                                 UserName = x.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y=>y.NickName).FirstOrDefault(),
                                 StartDate = x.StartDate,
                                 EndDate = x.EndDate
                             }).ToList();
                if (leave == null)
                {
                    throw new ArgumentException("No Data found!!");
                }

                return new Result()
                {
                    Message = string.Format("Leave Request"),
                    Status = Result.ResultStatus.success,
                    Data = leave,
                };
            }
        }

        public Result Update(object URId, int leaveId, Models.Employer.Organization.Staff.Leave.Approve value)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var leave = c.OrgStaffsLeaveApplications.SingleOrDefault(x => x.OrgStaffLeaveId == leaveId
                                                                          && x.SubUserOrganisation_StaffURId.OId == user.OId);
                if (leave == null)
                {
                    throw new ArgumentException("Data not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var duration = leave.EndDate.Subtract(leave.StartDate).Days + 1;
                var total = (value.Paid == null ? 0 : value.Paid) + (value.UnPaid == null ? 0 : value.UnPaid);
                if(total > duration || total < duration)
                {
                    throw new ArgumentException("Approve days not match with leave duration");
                }
                leave.LeaveStatusId = (int?)LeaveStatus.Accepted;
                leave.PaidDays = (value.Paid == null ? 0 : value.Paid);
                leave.UnPaidDays = (value.UnPaid == null ? 0 : value.UnPaid);
                leave.URId = (int)URId;
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Approve Successfully"),
                    Data = new
                    {
                        Id = leave.OrgStaffLeaveId,
                        Name = leave.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName
                    }
                };
            }
        }

        public Result Remove(object URId, int leaveId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var leave = c.OrgStaffsLeaveApplications.SingleOrDefault(x => x.OrgStaffLeaveId == leaveId
                                                                          && x.SubUserOrganisation_URId.OId == user.OId);
                if (leave == null)
                {
                    throw new ArgumentException("Data not found!!");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                leave.LeaveStatusId = (int?)LeaveStatus.Reject;
                leave.PaidDays = 0;
                leave.UnPaidDays = 0;
                leave.URId = (int)URId;
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Reject Successfully"),
                    Data = new
                    {
                        Id = leave.OrgStaffLeaveId,
                        Name = leave.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName
                    }
                };
            }
        }

    }
}
