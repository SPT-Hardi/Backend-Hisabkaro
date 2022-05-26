using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employee.Resume;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff.Salary
{
    public class SalaryDetails
    {
        public Result Deduction(object URId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);

                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "staff");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    var Deduction = (from x in c.OrgStaffsSalaryDetails
                                     where x.StaffURId == _URId.URId
                                     select new
                                     {
                                         Date = x.Date,
                                         Advance = x.Advance,
                                         Leave = x.OrgStaffLeave,
                                         Loan = x.LoanDeductionAmount,
                                         TotalDeduction = x.Advance + x.OrgStaffLeave + x.LoanDeductionAmount,
                                     }).ToList();


                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            Deduction = Deduction
                        },
                    };
                }
            }
        }
        public Result Payment(object URId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);

                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "staff");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    var Payment = (from x in c.OrgStaffsSalaryDetails
                                   where x.StaffURId == _URId.URId
                                   select new
                                   {
                                       Date = x.Date,
                                       TotalDeduction = x.Advance + x.OrgStaffLeave + x.LoanDeductionAmount,
                                       Salary = x.Salary,
                                       ActualSalary = x.ASalary,
                                   }).ToList();


                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            Payment = Payment
                        },
                    };
                }
            }
        }
        public Result OverTime(object URId,DateTime Date)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(Date);

                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "staff");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    List<OverTime> overTime = new List<OverTime>();
                                        
                    var OverTime = (from x in c.OrgStaffsOverTimeDetails
                                    where x.StaffURId == _URId.URId && x.OverTimeDate.Month == ISDT.Month && x.OverTimeDate.Year == ISDT.Year
                                    select x ).ToList();

                    OverTime.ForEach((x) => {
                        var Attendance = (from y in c.OrgStaffsAttendancesDailies
                                          where y.ChekIN.Value.Date == x.OverTimeDate && y.URId == _URId.URId
                                          select new { y.ChekIN, y.CheckOUT }).FirstOrDefault();

                        overTime.Add(new OverTime() { 
                            Time=x.OverTime,
                            Date=x.OverTimeDate,
                            CheckIn=Attendance.ChekIN.Value,
                            CheckOut=Attendance.CheckOUT.Value,
                            Hours = Attendance.CheckOUT == null ? null : (Attendance.CheckOUT.Value.TimeOfDay - Attendance.ChekIN.Value.TimeOfDay).ToString(),
                        });
                    });
                   
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            OverTime = overTime
                        },
                    };
                }
            }
        }
    }
}
