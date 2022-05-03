using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
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
                                             select y.FGUID).SingleOrDefault(),
                              }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = _staff
                };
            }
        }

        public Result Create(object URId, int StaffId, Models.Employer.Organization.Staff.Salary.SalaryDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == StaffId && x.SubUserOrganisation.OId == _URId.OId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    var _AttendDeduction = AttendDeduction(StaffId);
                    var _Bonus = Bonus(StaffId);
                    var _Advance = Advance(StaffId);
                    var _Loan = Loan(StaffId);
                    //var _CountAttend = Attendance(StaffId);
                    
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Bouns Give Successfully!"),
                        Data = new
                        {
                            AttedDeduction = _AttendDeduction,
                            Bouns=_Bonus,
                            Advance=_Advance,
                        }
                    };
                }
            }
        }

        public decimal AttendDeduction(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var salary = (from x in c.DevOrganisationsStaffs
                                 where x.URId == StaffURId
                                 select x.Salary).SingleOrDefault();

                    var leave = (from x in c.OrgStaffsLeaveApplications
                                 where x.StaffURId == StaffURId && x.StartDate.Month == DateTime.Now.Month - 1 && x.IsLeaveApproved== "Accepted"
                                 select x.UnPaidDays).Sum();

                    var Deduction=(leave * (salary/ 30));

                    scope.Complete();
                    return (decimal)Deduction ;

                }
            }
        }

        public decimal Bonus(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _bonus = (from x in c.OrgStaffsBonusDetails
                                 where x.StaffURId == StaffURId && x.Date.Month == DateTime.Now.Month - 1 
                                 select x.Amount).Sum();
                    
                    scope.Complete();
                    return (decimal)_bonus;

                }
            }
        }

        public decimal Advance(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _Advance = (from x in c.OrgStaffsAdvanceDetails
                                  where x.StaffURId == StaffURId && x.Date.Month == DateTime.Now.Month - 1
                                  select x.Amount).Sum();

                    scope.Complete();
                    return (decimal)_Advance;

                }
            }
        }

        public int Loan(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _Loan = (from x in c.OrgStaffsLoanDetails
                                    where x.StaffURId == StaffURId && x.RemainingAmt!=0  && x.Status==true
                                    select x).SingleOrDefault();
                    if (_Loan is null)
                    {
                        return 0;
                    }
                    int totalMonth = Math.Abs( 12 * (_Loan.StartDate.Year - _Loan.EndDate.Year) + _Loan.StartDate.Month - _Loan.EndDate.Month);
                                        
                    var t = _Loan.RemainingAmt - _Loan.MonthlyPay;

                    if (t <= _Loan.MonthlyPay)
                    {
                        _Loan.RemainingAmt = 0;
                    }
                    else
                    {
                        _Loan.RemainingAmt = t;
                    }
                    
                    c.SubmitChanges();
                    scope.Complete();
                    return 1;  
                }
            }
        }

        public int Attendance(int StaffURId)
        {
            return 1;
            //using (DBContext c = new DBContext())
            //{
            //    using (TransactionScope scope = new TransactionScope())
            //    {
            //        var atte = (from x in c.OrgStaffsAttendancesDailies
            //                    where x.URId == StaffURId && x.ChekIN.Value.Month == DateTime.Now.Month - 1
            //                    select x).ToList();

            //        var leave = (from x in c.OrgStaffsLeaveApplications
            //                     where x.StaffURId == StaffURId && x.StartDate.Month == DateTime.Now.Month - 1
            //                     select x.PaidDays).Sum();
            //        var totaldays = atte.Count() + leave;
            //        scope.Complete();
            //        return (int)totaldays;

            //    }
            //}
        }
    }
}
