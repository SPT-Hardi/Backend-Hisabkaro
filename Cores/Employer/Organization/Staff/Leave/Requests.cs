﻿using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Leave
{
    public class Requests
    {
        public Result Create(int URId, Models.Employer.Organization.Staff.Leave.Request value)
        {
            using(DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }

                var request = new OrgStaffsLeaveApplication()
                {
                    StaffURId = user.URId,
                    StartDate = value.StartDate,
                    EndDate = value.EndDate,
                    Reason = value.reason,
                    IsLeaveApproved = "Pending"
                };
                c.OrgStaffsLeaveApplications.InsertOnSubmit(request);
                c.SubmitChanges();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Leave Apply Successfully"),
                    Data = new
                    {
                        Id = request.OrgStaffLeaveId,
                        Name = user.SubUser.SubUsersDetail.FullName
                    }
                };
            }
        }

        public Result Get(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User not found!!");
                }   

                var leave = (from x in c.OrgStaffsLeaveApplications
                             orderby x.OrgStaffLeaveId descending
                             where x.StaffURId == user.URId
                             select new
                             {
                                 Id = x.OrgStaffLeaveId,
                                 Reason = x.Reason,
                                 StartDate =  x.StartDate,
                                 EndDate = x.EndDate,
                                 IsApprove = x.IsLeaveApproved
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
    }
}
