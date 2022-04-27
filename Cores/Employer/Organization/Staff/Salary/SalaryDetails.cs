using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Salary
{
    public class SalaryDetails
    {
        public Result One(int OId)
        {
            using (DBContext c = new DBContext())
            {
                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == OId
                              select new
                              {
                                  URId = x.URId,
                                  Name = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                  Profile = (from y in c.CommonFiles
                                             where y.FileId == x.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                             select y.FilePath).SingleOrDefault(),
                              }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = _staff
                };
            }
        }

        public Result Create(int URId, int StaffId, Models.Employer.Organization.Staff.Salary.SalaryDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId && x.SubRole.RoleName == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == StaffId && x.SubUserOrganisation.OId == _URId.OId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    var _CountAtted = Attendance(StaffId);



                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Bouns Give Successfully!"),
                    };
                }
            }
        }

        public int Attendance(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var atte = (from x in c.OrgStaffsAttendancesDailies
                                where x.URId == StaffURId && x.ChekIN.Value.Month == DateTime.Now.Month - 1
                                select x).Count();

                    var leave = (from x in c.OrgStaffsLeaveApplications
                                 where x.URId == StaffURId && x.StartDate.Month == DateTime.Now.Month - 1 
                                 select x.UnPaidDays);
                    scope.Complete();
                    return atte ;

                }
            }
        }
    }
}
