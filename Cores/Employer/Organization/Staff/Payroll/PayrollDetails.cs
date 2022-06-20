﻿using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Payroll;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Payroll
{
    public class PayrollDetails
    {
        public enum Component
        {
            HRA=1,
            NightAllowance =2,
            PF=3,
            ESI=4
        }

        //=========PF
        public Result PFOne(object URId)
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

                    view.Add(new View() { 
                        Staffset = new IntegerNullString() { Id = s.URId, Text = s.Name, }  ,
                        Profile = s.Profile,
                        MobileNumber=s.MobileNumber,
                        Hours = new HistoryByMonths().Get(URId, s.URId, ISDT).Data.TotalWorkingHourPerMonth,
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
        public Result PFCreate(object URId, Models.Employer.Organization.Staff.Payroll.PayrollDetail value)
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
        public Result ESIOne(object URId)
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
                        Staffset = new IntegerNullString() { Id = s.URId, Text = s.Name, },
                        Profile = s.Profile,
                        MobileNumber = s.MobileNumber,
                        Hours = new HistoryByMonths().Get(URId, s.URId, ISDT).Data.TotalWorkingHourPerMonth,
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
        public Result ESICreate(object URId, Models.Employer.Organization.Staff.Payroll.PayrollDetail value)
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
    }
}