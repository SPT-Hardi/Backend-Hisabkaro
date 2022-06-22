using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Salary;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static HIsabKaro.Cores.Employer.Organization.Salary.SalaryComponents;
using static HIsabKaro.Cores.Employer.Organization.Staff.Leave.Approves;

namespace HIsabKaro.Cores.Employer.Organization.Salary
{
    public class SalarySheets
    {
        public Result Pending(object URId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                    List<Pending> pending = new List<Pending>();
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

                    var _StaffSalary = (from x in c.PayrollSalarySlips
                                        where x.SubUserOrganisation.OId == _URId.OId && x.Month.Month == ISDT.Month-1 && x.Month.Year == (ISDT.Month == 1 ? ISDT.Year - 1 : ISDT.Year)
                                        select new { URId = x.StaffURId }).ToList();

                    var _Staff = (from x in c.DevOrganisationsStaffs
                                  where x.OId == _URId.OId
                                  select new { URId = x.URId }).ToList().Except(_StaffSalary);
                    var _s = (from x in _Staff
                              select new { x.URId }).ToList();

                    
                    _s.ForEach((x) => {
                        var _Salary = One(_URId.URId, x.URId, ISDT);
                        var _loan = OneLoan(x.URId, ISDT);
                        pending.Add(new Pending()
                        {
                            URId = x.URId,
                            Name = c.DevOrganisationsStaffs.Where(y => y.URId == x.URId).Select(y => y.NickName).FirstOrDefault(),
                            Salary = _Salary.Data.Salary-_loan,
                            Hours = new HistoryByMonths().Get(URId, x.URId, ISDT.AddMonths(-1)).Data.TotalWorkingHourPerMonth,
                        });
                    });

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = new { 
                            Total=pending.Sum(x=>x.Salary),
                            Staff=pending 
                        },
                    };
                }
            }
        }

        public Result Create(object URId ,int StaffURId)
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
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    var _Salary = One(_URId.URId, StaffURId, ISDT);
                    var _loan = CreateLoan(StaffURId, ISDT);


                    var slip = new PayrollSalarySlip()
                    {
                        StaffURId = StaffURId,
                        OId = _URId.OId,
                        GeneratedAt = ISDT.Date,
                        BasicAmount = _Salary.Data.ActualSalary,
                        Month = ISDT.Month==1 ? new DateTime(ISDT.Year-1, 12, 1) : new DateTime(ISDT.Year, ISDT.Month - 1,1),
                        NetPay=_Salary.Data.Salary - _loan.Data.Text,
                        AbsentDeduction= _Salary.Data.Leave,
                        LoanDeduction=_loan.Data.Text,
                        OverTimeEarning=_Salary.Data.OverTime,
                        URId=(int)URId,
                    };
                    c.PayrollSalarySlips.InsertOnSubmit(slip);
                    c.SubmitChanges();

                    var sliplist = slip;
                   
                    var Earning = c.PayrollStaffSalaryComponents.Where(x => x.URId == StaffURId && x.PayrollSalaryComponent.ComponentTypeId == (int)Component.Earning && (x.IsForMonth == true && x.Date.Value.Month == ISDT.Month - 1 && x.Date.Value.Year == (ISDT.Month == 1 ? ISDT.Year - 1 : ISDT.Year))).ToList();

                    Earning.ForEach(x => {
                        var VV = new PayrollSalarySlipsComponent
                        {
                            SalarySlipId = sliplist.SalarySlipId,
                            ComponentTypeId = x.PayrollSalaryComponent.ComponentTypeId,
                            Name = x.PayrollSalaryComponent.ComponentName,
                            Amount = x.Amount,
                        };
                        c.PayrollSalarySlipsComponents.InsertOnSubmit(VV);
                        c.SubmitChanges();

                    });

                    var Deduction = c.PayrollStaffSalaryComponents.Where(x => x.URId == StaffURId && x.PayrollSalaryComponent.ComponentTypeId == (int)Component.Deduction).ToList();
                    
                    Deduction.ForEach(x => {
                        var xx = new PayrollSalarySlipsComponent
                        {
                            SalarySlipId = sliplist.SalarySlipId,
                            ComponentTypeId = x.PayrollSalaryComponent.ComponentTypeId,
                            Name = x.PayrollSalaryComponent.ComponentName,
                            Amount = x.Amount,
                        };
                        c.PayrollSalarySlipsComponents.InsertOnSubmit(xx);
                        c.SubmitChanges();
                    });

                    scope.Complete();
                    return new Result() { Status = Result.ResultStatus.success, };

                }
            }  
        }

        public Result One(object URId, int StaffId,DateTime ISDT)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
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

                    var Earning = c.PayrollStaffSalaryComponents.
                        Where(x => x.URId == StaffId && x.PayrollSalaryComponent.ComponentTypeId==(int)Component.Earning &&
                        (x.IsForMonth==true && x.Date.Value.Month==ISDT.Month-1 && x.Date.Value.Year == (ISDT.Month == 1 ? ISDT.Year - 1 : ISDT.Year))).Select(x=>x.Amount).Sum();
                   

                    var Deduction= c.PayrollStaffSalaryComponents.Where(x => x.URId == StaffId && x.PayrollSalaryComponent.ComponentTypeId == (int)Component.Deduction).Select(x => x.Amount).Sum();

                    decimal Salary = (decimal)_Staff.Salary;
                    decimal _OverTime = OverTime(StaffId,ISDT, Salary);
                    decimal _Advance =  Advance(StaffId, ISDT);    
                    decimal _Leave =Leave(StaffId, Earning, ISDT);    
                    
                    decimal _Salary = Salary + Earning - Deduction - _Advance - _Leave ;

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Salary Give Successfully!"),
                        Data = new
                        {
                            OverTime = _OverTime,
                            Earning= Earning,
                            Deduction= Deduction,
                            Advance = _Advance,
                            Leave = _Leave,
                            ActualSalary = Salary,
                            Salary = Salary + Earning + _OverTime - Deduction - _Advance - _Leave ,
                        }
                    };
                }
            }
        }

        public decimal OverTime(int StaffURId, DateTime ISDT,decimal Salary)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OT = (from x in c.OrgStaffsAttendancesDailies
                               where x.URId == StaffURId && x.ChekIN.Month == ISDT.Month - 1 && (x.IsOvertimeFullDay==true || x.IsOvertimeHalfDay==true)
                               select x).ToList();
                    
                    var fullot = (from x in _OT
                                    where x.IsOvertimeFullDay == true
                                    select x).ToList().Count();
                    var fullday = fullot* (Salary / 30);


                    var halfot = (from x in _OT
                                    where x.IsOvertimeHalfDay== true
                                    select x).ToList().Count();
                    var halfday = halfot * (Salary / (30*2));

                    scope.Complete();
                    return (decimal)(fullday+ halfday);
                }
            }
        }
                
        public decimal Advance(int StaffURId, DateTime ISDT)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _Advance = (from x in c.OrgStaffsAdvanceDetails
                                    where x.StaffURId == StaffURId && x.Date.Month == ISDT.Month - 1 && x.Date.Year==(ISDT.Month==1?ISDT.Year-1:ISDT.Year) && x.IsEMI == false
                                    select x.Amount).FirstOrDefault();
                    scope.Complete();
                    return (decimal)(_Advance == null ? 0 : _Advance);

                }
            }
        }

        public decimal Leave(int StaffURId, decimal Earning, DateTime ISDT)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var salary = (from x in c.DevOrganisationsStaffs
                                  where x.URId == StaffURId
                                  select x.Salary).SingleOrDefault();

                    var leave = (from x in c.OrgStaffsLeaveApplications
                                where x.StaffURId == StaffURId && x.StartDate.Month == ISDT.Month - 1 && x.StartDate.Year == (ISDT.Month == 1 ? ISDT.Year - 1 : ISDT.Year) && x.LeaveStatusId == (int)LeaveStatus.Accepted
                                select x.UnPaidDays).ToList().Sum();

                    var Deduction = ((leave == null ? 0 : leave) * (((decimal)salary+Earning) / 30));   

                    scope.Complete();
                    return (decimal)Deduction;

                }
            }
        }
        
        public decimal OneLoan(int StaffURId, DateTime ISDT)
        {
            using (DBContext c = new DBContext())
            {
                var Loan = (from x in c.OrgStaffsLoanDetails
                            where x.StaffURId == StaffURId && x.IsLoanPending == true
                            select x).FirstOrDefault();
                if (Loan is null)
                {
                    return (decimal)0;
                }

                var Installment = (from x in c.OrgStaffLoanInstallmentDetails
                                   where x.LoanId == Loan.LoanId && x.IsInstallmentCompleted == false && x.Month == ISDT.AddMonths(-1).ToString("MMMM") && x.Year == ISDT.Year.ToString()
                                   select x.MonthlyPay).FirstOrDefault();

                return (decimal)(Installment == null ? 0 : Installment);

            }
        }

        public Result CreateLoan(int StaffURId,DateTime ISDT)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _Loan = (from x in c.OrgStaffsLoanDetails
                                 where x.StaffURId == StaffURId && x.RemainingAmt != 0 && x.IsLoanPending == true
                                 select x).SingleOrDefault();
                    if (_Loan is null)
                    {
                        scope.Complete();
                        return new Result
                        {
                            Status = Result.ResultStatus.success,
                            Message = string.Format(""),
                            Data = new { Id = 0, Text = 0.ToString() }
                        };
                    }
                    var installment = (from x in c.OrgStaffLoanInstallmentDetails
                                       where x.LoanId == _Loan.LoanId && x.IsInstallmentCompleted == false
                                       select x).ToList();

                    if (installment is null)
                    {
                        return new Result
                        {
                            Status = Result.ResultStatus.success,
                            Message = string.Format(""),
                            Data = new { Id = 0, Text = 0.ToString() }
                        };
                    }
                    var cut = (from x in installment
                               where x.Month == ISDT.AddMonths(-1).ToString("MMMM") && x.Year == (ISDT.Month == 1 ? (ISDT.Year - 1).ToString() : ISDT.Year.ToString()) && x.IsInstallmentCompleted == false
                               select x).FirstOrDefault();

                    if (installment.Count() == 1)
                    {
                        _Loan.RemainingAmt = (_Loan.RemainingAmt - cut.MonthlyPay) <= 0 ? 0 : _Loan.RemainingAmt - cut.MonthlyPay;
                        _Loan.IsLoanPending = false;

                        cut.IsInstallmentCompleted = true;
                        c.SubmitChanges();
                    }
                    else
                    {
                        var m = ISDT.AddMonths(-1).ToString("MMMM") + " " + ISDT.Year;

                        _Loan.RemainingAmt = _Loan.RemainingAmt - cut.MonthlyPay;

                        cut.IsInstallmentCompleted = true;
                        c.SubmitChanges();
                    }


                    scope.Complete();
                    return new Result
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format(""),
                        Data = new { Id = _Loan.LoanId, Text = cut.MonthlyPay/*.ToString() */}
                    };
                }
            }
        }

    }
}
