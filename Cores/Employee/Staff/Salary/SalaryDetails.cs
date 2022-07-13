using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employee.Resume;
using HIsabKaro.Models.Employer.Organization.Salary;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using static HIsabKaro.Cores.Employer.Organization.Salary.SalaryComponents;

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
                    SalarySlip salarySlip = new SalarySlip();

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

                    var deduction = (from x in c.PayrollSalarySlips
                                     where x.StaffURId == (int)URId
                                     select new Deduction()
                                     {
                                         Date=x.Month,
                                         Loan = (decimal)x.LoanDeduction,
                                         //Advance = (decimal)x.,
                                         Leave = (decimal)x.AbsentDeduction,
                                         PF = x.PayrollSalarySlipsComponents.FirstOrDefault(y => y.SalarySlipId == x.SalarySlipId && y.Name == "PF ( Provident Fund )").Amount,
                                         ESI = x.PayrollSalarySlipsComponents.FirstOrDefault(y => y.SalarySlipId == x.SalarySlipId && y.Name == "ESI (Employees' State Insurance Scheme)").Amount,

                                     }).SingleOrDefault();
                    salarySlip.deduction = deduction;
                    salarySlip.deduction.TotalDeduction = deduction.Loan + deduction.Leave + deduction.PF + deduction.ESI;

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Deduction Details"),
                        Data = new
                        {
                            Deduction = salarySlip.deduction
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

                    List<Payment> payments = new List<Payment>();


                    var Payment = (from x in c.PayrollSalarySlips
                                   where x.StaffURId == _URId.URId
                                   select x).ToList();

                    Payment.ForEach(x =>
                        payments.Add(new Payment(){
                            Date = x.Month,
                            Loan = (decimal)x.LoanDeduction,
                            TotalDeduction = (decimal)x.AbsentDeduction + (decimal)x.LoanDeduction + Convert.ToDecimal(x.PayrollSalarySlipsComponents.Where(y => y.SalarySlipId == x.SalarySlipId && y.ComponentTypeId == (int)Component.Deduction).Select(y => y.Amount).Sum()),
                            NetPay = x.NetPay,
                            Salary = x.BasicAmount,
                        })
                    );
                    

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Payment Details"),
                        Data = new
                        {
                            Payment = payments ,
                        },
                    };
                }
            }
        }          
    }
}
