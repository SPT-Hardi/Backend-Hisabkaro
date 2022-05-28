using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Salary;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Salary
{
    public class SalaryDetails
    {
        public Result SalarySlip(object URId)
        {
            using (DBContext c = new DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                SalarySlip salarySlip = new SalarySlip();
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if(user == null)
                {
                    throw new ArgumentException("User doesn't exist");
                }

                var org = (from x in c.DevOrganisations
                               where x.OId == user.OId
                               select new Organisation()
                               {
                                   OId = x.OId,
                                   OrganizationName = x.OrganisationName,
                                   Logo = x.CommonFile_LogoFileId.FilePath,
                                   PAN = x.PAN,
                                   GST = x.GSTIN
                               }).SingleOrDefault();
                salarySlip.organisation = org;

                var employeeDetail = (from x in c.DevOrganisationsStaffs
                                      where x.URId == (int)URId
                                      select new EmployeeDetail()
                                      {
                                          EmployeeId = (int)x.SId,
                                          AccountNo = x.DevOrganisationsStaffsBankDetail.AccountNumber,
                                          HolderName = x.DevOrganisationsStaffsBankDetail.Name,
                                          IFSC = x.DevOrganisationsStaffsBankDetail.IFSCCode,
                                          PAN = "41529632145",
                                          AadharCard = "967412859632"
                                      }).SingleOrDefault();
                salarySlip.employeeDetail = employeeDetail;

                salarySlip.attendanceDetail.PaidLeave = (int)(from y in c.OrgStaffsLeaveApplications
                                                              where y.StaffURId == (int)URId && y.StartDate.Month == DateTime.Now.Month - 1 && y.SubFixedLookup.FixedLookupFormatted == "Accepted"
                                                              select y.PaidDays).Sum();
                salarySlip.attendanceDetail.Present = (from y in c.OrgStaffsAttendancesDailies
                                                       where y.URId == (int)URId && y.ChekIN.Value.Month == DateTime.Now.Month - 1 
                                                       select y).ToList().Count();
                salarySlip.attendanceDetail.Absent = (DateTime.DaysInMonth(ISDT.Year, ISDT.Month) -1) - (from y in c.OrgStaffsAttendancesDailies
                                                                                                    where y.URId == (int)URId && y.ChekIN.Value.Month == DateTime.Now.Month - 1
                                                                                                    select y).ToList().Count();
                salarySlip.attendanceDetail.Workingdays = salarySlip.attendanceDetail.Present + salarySlip.attendanceDetail.Absent;

              

                var earning = (from x in c.OrgStaffsSalaryDetails
                                      where x.StaffURId == (int)URId && x.Date.Month == ISDT.Month
                                      select new Earning()
                                      {
                                         Salary = (decimal)x.ASalary,
                                         Overtime = (decimal)x.OverTime,
                                         Bonus = (decimal)x.Bonus,
                                         TotalEarning = (decimal)(x.ASalary + x.OverTime + x.Bonus)
                                      }).SingleOrDefault();
                salarySlip.earning = earning;

                var deduction = (from x in c.OrgStaffsSalaryDetails
                               where x.StaffURId == (int)URId && x.Date.Month == ISDT.Month
                               select new Deduction()
                               {
                                  Loan = (decimal)x.LoanDeductionAmount,
                                  Advance = (decimal)x.Advance,
                                  Leave = (decimal)x.OrgStaffLeave,
                                  TotalDeduction = (decimal)(x.LoanDeductionAmount + x.Advance + x.OrgStaffLeave)
                               }).SingleOrDefault();
                salarySlip.deduction = deduction;

                salarySlip.NetPay = ((earning == null ? 0 : salarySlip.earning.TotalEarning) - (deduction == null ? 0 : salarySlip.deduction.TotalDeduction));

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("SalarySlip"),
                    Data = salarySlip,
                };
            }
        }

        public Result Create(object URId, int StaffId)
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

                    var _StaffSalary = (from x in c.OrgStaffsSalaryDetails
                                        where x.StaffURId == StaffId && x.Date.Month == ISDT.Month
                                        select x).FirstOrDefault();
                    if(_StaffSalary is not null)
                    {
                        throw new ArgumentException("Salary Alredy Paid.");
                    }

                    var _Salary = One(URId, StaffId);

                    var _SalaryDetail = new OrgStaffsSalaryDetail() {
                        OverTime = _Salary.Data.OverTime,
                        Bonus = _Salary.Data.Bonus ,
                        Advance = _Salary.Data.Advance  ,                              
                        OrgStaffLeave = _Salary.Data.Leave ,
                        LoanId = _Salary.Data.LoanId,
                        LoanDeductionAmount = _Salary.Data.Loan ,
                        Salary = _Salary.Data.Salary,
                        Date = ISDT,
                        StaffURId=StaffId,
                        URId=(int)URId,
                        ASalary= _Salary.Data.ActualSalary,
                    };
                    c.OrgStaffsSalaryDetails.InsertOnSubmit(_SalaryDetail);
                    c.SubmitChanges();
                    var __Salary = _SalaryDetail.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(x => x.Salary);
                    var Name = _SalaryDetail.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y => y.NickName).FirstOrDefault();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Salary Give Successfully!"),
                        Data = new
                        {
                            Id = _SalaryDetail.SalaryId,
                            Salary=_SalaryDetail.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(x=>x.Salary),
                            Name=_SalaryDetail.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y=>y.NickName).FirstOrDefault()
                        }
                    };
                }
            }
        }

        public Result One(object URId, int StaffId)
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

                    decimal _OverTime = OverTime(StaffId) == null ? 0 : OverTime(StaffId);
                    decimal _Bonus = Bonus(StaffId) == null ? 0 :Bonus(StaffId);
                    decimal _Advance = Advance(StaffId) == null ? 0 :Advance(StaffId);
                    decimal _Leave = Leave(StaffId) == null ? 0 :Leave(StaffId);
                    var _Loan = Loan(StaffId);

                    decimal loan = decimal.Parse(_Loan.Data.Text);
                    decimal salary = (decimal)_Staff.Salary;

                    decimal _Salary = salary + _OverTime + _Bonus - _Advance - _Leave - loan;

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Salary Give Successfully!"),
                        Data = new
                        {
                            OverTime = _OverTime,
                            Bonus = _Bonus,
                            Advance = _Advance,
                            Leave = _Leave,
                            LoanId=_Loan.Data.Id,
                            Loan = decimal.Parse(_Loan.Data.Text),
                            ActualSalary= salary,
                            Salary = salary + _OverTime + _Bonus - _Advance - _Leave - loan,
                        }
                    };
                }
            }
        }

        public decimal OverTime(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _OverTime = (from x in c.OrgStaffsOverTimeDetails
                                     where x.StaffURId == StaffURId && x.OverTimeDate.Month == DateTime.Now.Month - 1
                                     select x.Amount).Sum();

                    scope.Complete();
                    return (decimal)(_OverTime == null ? 0 : _OverTime);

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
                    return (decimal)(_bonus == null ? 0 : _bonus);

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
                    return (decimal)(_Advance==null?0:_Advance);

                }
            }
        }

        public decimal Leave(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var salary = (from x in c.DevOrganisationsStaffs
                                  where x.URId == StaffURId
                                  select x.Salary).SingleOrDefault();

                    var leave = (from x in c.OrgStaffsLeaveApplications
                                 where x.StaffURId == StaffURId && x.StartDate.Month == DateTime.Now.Month - 1 && x.SubFixedLookup.FixedLookupFormatted == "Accepted"
                                 select x.UnPaidDays).Sum();

                    var Deduction = ((leave == null ? 0 : leave) * (salary / 30));

                    scope.Complete();
                    return (decimal)Deduction;

                }
            }
        }

        public Result Loan(int StaffURId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _Loan = (from x in c.OrgStaffsLoanDetails
                                    where x.StaffURId == StaffURId && x.RemainingAmt!=0  && x.IsLoanPending == true
                                    select x).SingleOrDefault();
                    if(_Loan is null)
                    {
                        scope.Complete();
                        return new Result
                        {
                            Status = Result.ResultStatus.success,
                            Message = string.Format(""),
                            Data = new  { Id =0, Text =0.ToString()}
                        };
                    }
                    var sal = (from x in c.OrgStaffsSalaryDetails
                               where x.LoanId == _Loan.LoanId
                               select x).Count();

                    int totalMonth = Math.Abs(12 * (_Loan.StartDate.Year - _Loan.EndDate.Year) + _Loan.StartDate.Month - _Loan.EndDate.Month);

                    decimal tot = 0;
                    if (sal+1 == totalMonth)
                    {
                        tot = (decimal)_Loan.RemainingAmt;
                        _Loan.RemainingAmt = 0;
                        _Loan.IsLoanPending = false;
                        c.SubmitChanges();
                    }
                    else
                    {
                        tot = (decimal)_Loan.MonthlyPay;
                        _Loan.RemainingAmt = _Loan.RemainingAmt - _Loan.MonthlyPay;
                        c.SubmitChanges();
                    }

                    scope.Complete();
                    return new Result {
                        Status=Result.ResultStatus.success,
                        Message = string.Format(""),
                        Data = new { Id=_Loan.LoanId,Text=tot.ToString()}
                    };  
                }
            }
        }

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
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    var _StaffSalary = (from x in c.OrgStaffsSalaryDetails
                                        where x.SubUserOrganisation_StaffURId.OId == _URId.OId && x.Date.Month == ISDT.Month
                                        select new { Id= x.StaffURId }).ToList();

                    var _Staff = (from x in c.DevOrganisationsStaffs
                                  where x.OId == _URId.OId
                                  select new { Id= x.URId }).ToList().Except(_StaffSalary);
                    var _s = (from x in _Staff
                              select new { x.Id}).ToList();
                    
                    _s.ForEach((x) => {
                        var _Salary = One(_URId.URId, x.Id);
                        
                        pending.Add(new Pending() { 
                            URId=x.Id,
                            Name= c.DevOrganisationsStaffs.Where(y => y.URId == x.Id).Select(y => y.NickName).FirstOrDefault(),
                            Salary= _Salary.Data.Salary,
                            Hours = new HistoryByMonths().Get(URId, x.Id, ISDT.AddMonths(-1)).Data.TotalWorkingHourPerMonth,
                        });
                    });

                    var totalSalary = pending.Sum(x=>x.Salary);

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Salary Give Successfully!"),
                        Data=new {
                            Total=totalSalary,
                            Staff=pending
                        },
                    };
                }
            }
        }
        
    }
}