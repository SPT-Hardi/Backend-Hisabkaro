﻿using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Leave
{
    public class Approves
    {
        public Result Get(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                if(user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var leave = (from x in c.OrgStaffsLeaveApplications
                             where x.SubUserOrganisation.OId == user.OId && x.IsLeaveApproved == "Pending"
                             select new
                             {
                                 Id = x.OrgStaffLeaveId,
                                 UserName = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                 StartDate = x.StartDate,
                                 EndDate = x.EndDate,
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

        public Result Update(int URId, int leaveId, Models.Employer.Organization.Staff.Leave.Approve value)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
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

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                leave.IsLeaveApproved = "Accepted";
                leave.IsPaidLeave = value.Ispaid;
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Approve Successfully"),
                    Data = new
                    {
                        Id = leave.OrgStaffLeaveId,
                        Name = leave.SubUserOrganisation.SubUser.SubUsersDetail.FullName
                    }
                };
            }
        }

        public Result Remove(int URId, int leaveId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
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

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                leave.IsLeaveApproved = "Reject";
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Reject Successfully"),
                    Data = new
                    {
                        Id = leave.OrgStaffLeaveId,
                        Name = leave.SubUserOrganisation.SubUser.SubUsersDetail.FullName
                    }
                };
            }
        }

    }
}
