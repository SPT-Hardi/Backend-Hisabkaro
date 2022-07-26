using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Salary;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Salary
{
    public class SalaryComponents
    {
        public enum Component
        {
            Earning=101,
            Deduction=102,

            HRA = 1,
            NightAllowance = 2,
            PF = 3,
            ESI = 4,
            Bonus = 5,
            Incentive = 6,
            Allowance  = 7,
            Advance=8
        }

        //=========PF
        public Result PF(object URId)
        {

            using (DBContext c = new DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                List<View> view = new List<View>();

                var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var staff = (from x in c.PayrollStaffSalaryComponents
                             where x.SalaryComponentId == (int)Component.PF
                             select new { URId = x.URId }).ToList();

                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == _User.OId
                              select new { URId = x.URId }).ToList().Except(staff);
                
                foreach (var item in _staff)
                {
                    var s = (from y in c.DevOrganisationsStaffs
                             where y.URId == item.URId
                             select new
                             {
                                 URId = y.URId,
                                 Name = y.NickName,
                                 Profile = (from z in c.CommonFiles
                                            where z.FileId == y.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                            select z.FGUID).SingleOrDefault(),
                                 MobileNumber = y.SubUserOrganisation.SubUser.MobileNumber
                             }).FirstOrDefault();

                    view.Add(new View()
                    {
                        Staff = new IntegerNullString() { Id = s.URId, Text = s.Name, },
                        Profile = s.Profile,
                        MobileNumber = s.MobileNumber,
                        //Hours = new HistoryByMonths().Get(URId, s.URId, ISDT).Data.TotalWorkingHourPerMonth,
                    });

                }

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = new { view }
                };
            }
        }
        public Result PF(object URId, Models.Employer.Organization.Salary.SalaryComponent value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    value.StaffLists.ForEach((x) => {
                        var Staff = c.SubUserOrganisations.SingleOrDefault(y => y.URId == (int)x.Staff.Id);
                        if (Staff.OId != _User.OId)
                        {
                            throw new ArgumentException($"{x.Staff.Text} Does not Exist!");
                        }
                        var Pf = c.PayrollStaffSalaryComponents.SingleOrDefault(y => y.URId == (int)Component.PF);
                        if (Staff.OId != _User.OId)
                        {
                            throw new ArgumentException($"{x.Staff.Text} Already Give PF!");
                        }
                    });

                    var PF = value.StaffLists.Where(x => x.Status == true).Select(x => new HisabKaroContext.PayrollStaffSalaryComponent()
                    {
                        SalaryComponentId = (int)Component.PF,
                        URId = (int)x.Staff.Id,
                        Amount = value.Amount,
                    }).ToList();

                    c.PayrollStaffSalaryComponents.InsertAllOnSubmit(PF);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("PF Give Successfully!"),
                    };
                }
                return new Result() { };
            }
        }
        //=========ESI
        public Result ESI(object URId)
        {

            using (DBContext c = new DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                List<View> view = new List<View>();

                var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var staff = (from x in c.PayrollStaffSalaryComponents
                             where x.SalaryComponentId == (int)Component.ESI
                             select new
                             {
                                 URId = x.URId,
                             }).ToList();

                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == _User.OId
                              select new
                              {
                                  URId = x.URId,
                              }).ToList().Except(staff);

                foreach (var item in _staff)
                {
                    var s = (from y in c.DevOrganisationsStaffs
                             where y.URId == item.URId
                             select new
                             {
                                 URId = y.URId,
                                 Name = y.NickName,
                                 Profile = (from z in c.CommonFiles
                                            where z.FileId == y.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                            select z.FGUID).SingleOrDefault(),
                                 MobileNumber = y.SubUserOrganisation.SubUser.MobileNumber
                             }).FirstOrDefault();

                    view.Add(new View()
                    {
                        Staff = new IntegerNullString() { Id = s.URId, Text = s.Name, },
                        Profile = s.Profile,
                        MobileNumber = s.MobileNumber,
                        //Hours = new HistoryByMonths().Get(URId, s.URId, ISDT).Data.TotalWorkingHourPerMonth,
                    });

                }

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = new { view }
                };
            }
        }
        public Result ESI(object URId, Models.Employer.Organization.Salary.SalaryComponent value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    value.StaffLists.ForEach((x) => {
                        var Staff = c.SubUserOrganisations.SingleOrDefault(y => y.URId == (int)x.Staff.Id);
                        if (Staff.OId != _User.OId)
                        {
                            throw new ArgumentException($"{x.Staff.Text} Already Give ESI !");
                        }
                        var Pf = c.PayrollStaffSalaryComponents.SingleOrDefault(y => y.URId == (int)Component.ESI);
                        if (Staff.OId != _User.OId)
                        {
                            throw new ArgumentException($"{x.Staff.Text} Already Give ESI!");
                        }
                    });

                    var PF = value.StaffLists.Where(x => x.Status == true).Select(x => new HisabKaroContext.PayrollStaffSalaryComponent()
                    {
                        SalaryComponentId = (int)Component.ESI,
                        URId = (int)x.Staff.Id,
                        Amount = value.Amount,
                    }).ToList();

                    c.PayrollStaffSalaryComponents.InsertAllOnSubmit(PF);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("ESI Give Successfully!"),
                    };
                }
            }
        }

        //=========Allowance,Other Incentive,Bonus,Night Allowance 
        public Result Allowance(object URId)
        {

            using (DBContext c = new DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                List<View> view = new List<View>();

                var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var staff = (from x in c.PayrollStaffSalaryComponents
                             where x.SalaryComponentId == (int)Component.ESI
                             select new
                             {
                                 URId = x.URId,
                             }).ToList();

                var _staff = (from x in c.DevOrganisationsStaffs
                              where x.OId == _User.OId
                              select new
                              {
                                  URId = x.URId,
                              }).ToList();//.Except(staff);

                foreach (var item in _staff)
                {
                    var s = (from y in c.DevOrganisationsStaffs
                             where y.URId == item.URId
                             select new
                             {
                                 URId = y.URId,
                                 Name = y.NickName,
                                 Profile = (from z in c.CommonFiles
                                            where z.FileId == y.SubUserOrganisation.SubUser.SubUsersDetail.FileId
                                            select z.FGUID).SingleOrDefault(),
                                 MobileNumber = y.SubUserOrganisation.SubUser.MobileNumber
                             }).FirstOrDefault();

                    view.Add(new View()
                    {
                        Staff = new IntegerNullString() { Id = s.URId, Text = s.Name, },
                        Profile = s.Profile,
                        MobileNumber = s.MobileNumber,
                        //Hours = new HistoryByMonths().Get(URId, s.URId, ISDT).Data.TotalWorkingHourPerMonth,
                    });

                }

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("View"),
                    Data = new { view }
                };
            }
        }
        public Result Allowance(object URId, Models.Employer.Organization.Salary.SalaryEarningComponent value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "admin");
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    value.StaffLists.ForEach((x) => {
                        var Staff = c.SubUserOrganisations.SingleOrDefault(y => y.URId == (int)x.Staff.Id );
                        var a = Staff.PayrollStaffSalaryComponents.SingleOrDefault(z => z.SalaryComponentId == value.PaymentType.Id && z.Date.Value.Month == value.Date.Month && z.Date.Value.Year == value.Date.Year);
                        if (Staff.OId != _User.OId )
                        {
                            throw new ArgumentException($"{x.Staff.Text} Not Exist!");
                        }
                        if (x.Status == true && a != null )
                        {
                            throw new ArgumentException($"{x.Staff.Text} Already Give {value.PaymentType.Text} For This Month !");
                        }                            
                    });

                    var PF = value.StaffLists.Where(x => x.Status == true).Select(x => new HisabKaroContext.PayrollStaffSalaryComponent()
                    {
                        SalaryComponentId = (int)value.PaymentType.Id,
                        URId = (int)x.Staff.Id,
                        Amount = value.Amount,
                        Date=value.Date,
                        Description=value.Description,
                        IsForMonth=true
                    }).ToList();

                    c.PayrollStaffSalaryComponents.InsertAllOnSubmit(PF);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"{value.PaymentType.Text} Give Successfully!"),
                    };
                }
            }
        }
    }
}
