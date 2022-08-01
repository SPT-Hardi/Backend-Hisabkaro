using HIsabKaro.Cores.Common;
using HIsabKaro.Cores.Common.MailService;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.MailService;
using HIsabKaro.Services;
using HisabKaroContext;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using static HIsabKaro.Cores.Developer.Subscriber.Users;

namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class StaffDetails
    {
        public enum SalaryType 
        {
            Yearly=48,
            Monthly=49,
            Daily=50,
            Hourly=51
        }
        public Result Create(object URId, Models.Employer.Organization.Staff.StaffDetailList value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var userOrganization = (from x in c.SubUserOrganisations where x.URId == (int)URId select x).FirstOrDefault();
                    if (userOrganization == null) 
                    {
                        throw new ArgumentException("User not exist!");
                    }
                    value.StaffDetails.ForEach((v) =>
                     {
                         var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == v.Organization.Id);
                         if (_OId is null)
                         {
                             throw new ArgumentException("Organization Does Not Exits!");
                         }
                         if (_OId.SubUser.UId != userOrganization.SubUser.UId) 
                         {
                             throw new ArgumentException("Not authorized!");
                         }
                         var _OrgRole = (from x in c.SubRoles where x.OId == v.Organization.Id && x.LoginTypeId ==(int)LoginType.Staff select x).FirstOrDefault();
                         var _subUser = c.SubUsers.SingleOrDefault(x => x.MobileNumber == v.MobileNumber);

                         if (_subUser is not null)
                         {
                             var _subUserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
                             if (_subUserOrg is not null)
                             {
                                 throw new ArgumentException($"Staff Member Are Already Exits In {_subUserOrg.DevOrganisation.OrganisationName}!");
                             }
                             else
                             {
                                 var _userOrg = new SubUserOrganisation()
                                 {
                                     UId = _subUser.UId,
                                     OId = _OId.OId,
                                     RId = _OrgRole.RId
                                 };
                                 c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                                 c.SubmitChanges();
                             }
                         }
                         else
                         {
                             var _user = new SubUser()
                             {
                                 MobileNumber = v.MobileNumber,
                                 DefaultLanguageId = 42,
                                 DefaultLoginTypeId = 20
                             };
                             c.SubUsers.InsertOnSubmit(_user);
                             c.SubmitChanges();

                             var _userDetail = new SubUsersDetail()
                             {
                                 UId = _user.UId,
                                 FullName = v.Name
                             };
                             c.SubUsersDetails.InsertOnSubmit(_userDetail);
                             c.SubmitChanges();

                             var _userOrg = new SubUserOrganisation()
                             {
                                 UId = _user.UId,
                                 OId = _OId.OId,
                                 RId = _OrgRole.RId
                             };
                             c.SubUserOrganisations.InsertOnSubmit(_userOrg);
                             c.SubmitChanges();
                             _subUser = _user;
                         }
                         var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);

                         var _Sid = (from x in c.DevOrganisationsStaffs
                                     where x.OId == _OId.OId
                                     select x).Max(x => x.StaffCardId);

                         if (_Sid == 0)
                             _Sid = 1;
                         else
                             _Sid += 1;
                         var staff = new DevOrganisationsStaff()
                         {
                             NickName = v.Name,
                             StaffURId = _URID.URId,
                             OId = (int)v.Organization.Id,
                             SalaryTypeId = v.SalaryType.Id,
                             BasicSalary = v.BasicSalary,
                             StaffCardId = _Sid,
                             CreateDate = ISDT,
                             IsJoined=false,
                             TotalSalaryAmount=v.TotalSalaryAmount
                         };
                         if (v.SalaryType.Id == (int)SalaryType.Monthly || v.SalaryType.Id == (int)SalaryType.Yearly)
                             staff.HRA = v.HRA;

                         c.DevOrganisationsStaffs.InsertOnSubmit(staff);
                         c.SubmitChanges();


                         //----------------------------Advance-----------------------------------------------//
                         v.Advances.ForEach((z) =>
                         {
                             if (z.AdvanceAmount != null || z.AdvanceAmount!=0)
                             {
                                 var advance = new OrgStaffsAdvanceDetail()
                                 {
                                     AdminURId=(int)URId,
                                     Amount=z.AdvanceAmount==null ? 0 : (decimal)z.AdvanceAmount,
                                     FinalAmountWithInterest=z.FinalAmountWithInterest,
                                     CompleteDate=z.EndDate,   //Can be update in future if one pay late
                                     CreateDate=DateTime.Now,
                                     Description=z.Description,
                                     EndDate=z.EndDate,    //In future never update
                                     InterestRate=z.InterestRate,
                                     IsCompleted=false,
                                     IsEMI=z.IsEMI,
                                     LastUpdated=DateTime.Now,
                                     StaffURId=_URID.URId,
                                     StartDate=z.StartDate,
                                     TotalMonths=z.TotalMonths,
                                 };
                                 c.OrgStaffsAdvanceDetails.InsertOnSubmit(advance);
                                 c.SubmitChanges();

                                 c.OrgStaffAdvanceInstallments.InsertAllOnSubmit(z.EMIPerMonths.Where(z => z.EMIAmount != null || z.EMIAmount != 0).Select(z => new OrgStaffAdvanceInstallment()
                                 {
                                     AdvanceId=advance.AdvanceId,
                                     InstallmentAmount=z.InstallmentAmount,
                                     TotalAmountPaid=z.TotalAmountPaid,
                                     TotalRemainAmount=z.TotalRemainAmount,
                                     CompleteDate=z.EndDate,       //can be updated
                                     Created=DateTime.Now,
                                     InstallmentEndDate=z.EndDate,   //cant be updated
                                     InstallmentMonth=z.InstallmentMonth,
                                     InstallmentStartDate=z.StartDate,
                                     IsCompleted=false,
                                     LastUpdated=DateTime.Now
                                 }));
                                 c.SubmitChanges();
                                 
                             }
                         });
                     });
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("View"),
                        Data = new
                        {
                            //OrgCode = _OId.OrgCode,
                            //OId = value.Organization.Id,
                        }
                    };
                }
            }
        }

        //public Result One(int OId)
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        var _staff = (from x in c.DevOrganisationsStaffs
        //                      where x.OId == OId
        //                      select new
        //                      {
        //                          URId=x.URId,
        //                          Name = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
        //                          Profile = (from y in c.CommonFiles
        //                                     where y.FileId == x.SubUserOrganisation.SubUser.SubUsersDetail.FileId
        //                                     select y.FGUID).SingleOrDefault(),
        //                      }).ToList();
        //        return new Result()
        //        {
        //            Status = Result.ResultStatus.success,
        //            Message = string.Format("View"),
        //            Data = _staff
        //        };
        //    }
        //}

        //public Result Create(object URId,Models.Employer.Organization.Staff.StaffDetail value)
        //{
        //    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.Id);
        //            if (_OId is null)
        //            {
        //                throw new ArgumentException("Organization Does Not Exits!");
        //            }

        //            var _Number = value.MobileNumber == value.AMobileNumber;
        //            if (_Number is true)
        //            {
        //                throw new ArgumentException("MoileNumber and AlternetNumber Are Same!");
        //            }

        //            var _OrgRole = (from x in c.SubRoles where x.OId == value.Organization.Id && x.RoleName.ToLower() == "staff" select x).FirstOrDefault();
        //            if (_OrgRole is null)
        //            {
        //                var _role = new SubRole()
        //                {
        //                    OId = _OId.OId,
        //                    RoleName = "staff",
        //                    LoginTypeId = 20,                        
        //                };
        //                c.SubRoles.InsertOnSubmit(_role);
        //                c.SubmitChanges();
        //                _OrgRole = _role;
        //            }

        //            var _subUser = c.SubUsers.SingleOrDefault(x => x.MobileNumber == value.MobileNumber);

        //            if (_subUser is not null)
        //            {
        //                var _subUserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
        //                if (_subUserOrg is not null)
        //                {
        //                    throw new ArgumentException($"Staff Member Are Already Exits In {_subUserOrg.DevOrganisation.OrganisationName}!");
        //                }
        //                else
        //                {
        //                    var _userOrg = new SubUserOrganisation()
        //                    {
        //                        UId = _subUser.UId,
        //                        OId = _OId.OId,
        //                        RId = _OrgRole.RId
        //                    };
        //                    c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                    c.SubmitChanges();
        //                }
        //            }
        //            else
        //            {
        //                var _user = new SubUser()
        //                {
        //                    MobileNumber = value.MobileNumber,
        //                    DefaultLanguageId = 1,
        //                    DefaultLoginTypeId = 20
        //                };
        //                c.SubUsers.InsertOnSubmit(_user);
        //                c.SubmitChanges();

        //                var _userDetail = new SubUsersDetail()
        //                {
        //                    UId = _user.UId,
        //                    FullName = value.Name,
        //                    Email = value.Email,
        //                    AMobileNumber = value.AMobileNumber
        //                };
        //                c.SubUsersDetails.InsertOnSubmit(_userDetail);
        //                c.SubmitChanges();

        //                var _userOrg = new SubUserOrganisation()
        //                {
        //                    UId = _user.UId,
        //                    OId = _OId.OId,
        //                    RId = _OrgRole.RId
        //                };
        //                c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                c.SubmitChanges();
        //                _subUser = _user;
        //            }

        //            var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);

        //            var _Sid = (from x in c.DevOrganisationsStaffs
        //                      where x.OId == _OId.OId
        //                      select x).Max(x => x.SId);

        //            if (_Sid == null)
        //            {
        //                _Sid = 1;
        //            }
        //            else
        //            {
        //                _Sid += 1;
        //            }
        //            var s = _Sid;

        //            var staff = new DevOrganisationsStaff()
        //            {
        //                NickName=value.Name,
        //                URId = _URID.URId,
        //                OId = (int)value.Organization.Id,
        //                ShiftTimeId = value.ShiftTiming.Id ==0?null:value.ShiftTiming.Id,
        //                Salary = value.Salary,
        //                IsOpenWeek = value.IsOpenWeek,
        //                SId = _Sid,
        //                Status = false,
        //                CreateDate = ISDT,
        //            };
        //            if (value.IsOpenWeek == false)
        //            {
        //                staff.WeekOffOneDay = value.WeekOff1.Id;
        //                staff.WeekOffSecondDay = value.WeekOff2.Id;
        //            }
        //            c.DevOrganisationsStaffs.InsertOnSubmit(staff);
        //            c.SubmitChanges();

        //            //if (value.Email != null)
        //            //{
        //            //    var mailRequest = new Models.Common.MailService.MailRequest() { 
        //            //        ToEmail=value.Email,
        //            //        Subject=$"Welcome To {_URID.DevOrganisation.OrganisationName}",
        //            //        Body=$"Hi <B>{value.Name}<B> <BR> Welcome to the team! We’re thrilled to have you at {_URID.DevOrganisation.OrganisationName}.We know you’re going to be a valuable asset to our company and can’t wait to see what you accomplish.",
        //            //    };
        //            //    Cores.Common.MailService.MailServices mailService = new Cores.Common.MailService.MailServices(_mailSetting);
        //            //    var mail = mailService.Create(mailRequest);

        //            //}
        //            scope.Complete();

        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format("Staff Add Successfully!"),
        //                Data = new
        //                {
        //                    OrgCode=_OId.OrgCode,
        //                    OId = value.Organization.Id,
        //                }
        //            };
        //        }
        //    }
        //}

        //public Result BulkCreate(object URId,List<Models.Employer.Organization.Staff.StaffDetail> value)
        //{
        //    var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
        //    using (DBContext c = new DBContext())
        //    {
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            value.ForEach((v) =>
        //            {
        //                var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == v.Organization.Id);
        //                if (_OId is null)
        //                {
        //                    throw new ArgumentException("Organization Does Not Exits!");
        //                }

        //                var _Number = v.MobileNumber == v.AMobileNumber;
        //                if (_Number is true)
        //                {
        //                    throw new ArgumentException("MoileNumber and AlternetNumber Are Same!");
        //                }

        //                var _OrgRole = (from y in c.SubRoles where y.OId == v.Organization.Id && y.RoleName.ToLower() == "staff" select y).FirstOrDefault();
        //                if (_OrgRole is null)
        //                {
        //                    var _role = new SubRole()
        //                    {
        //                        OId = _OId.OId,
        //                        RoleName = "staff",
        //                        LoginTypeId = 20,
        //                    };
        //                    c.SubRoles.InsertOnSubmit(_role);
        //                    c.SubmitChanges();
        //                    _OrgRole = _role;
        //                }

        //                var _subUser = c.SubUsers.SingleOrDefault(y => y.MobileNumber == v.MobileNumber);

        //                if (_subUser is not null)
        //                {
        //                    var _subUserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _subUser.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);
        //                    if (_subUserOrg is not null)
        //                    {
        //                        throw new ArgumentException($"Staff Member Are Already Exits In {_subUserOrg.DevOrganisation.OrganisationName}!");
        //                    }
        //                    else
        //                    {
        //                        var _userOrg = new SubUserOrganisation()
        //                        {
        //                            UId = _subUser.UId,
        //                            OId = _OId.OId,
        //                            RId = _OrgRole.RId
        //                        };
        //                        c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                        c.SubmitChanges();
        //                    }
        //                }
        //                else
        //                {
        //                    var _user = new SubUser()
        //                    {
        //                        MobileNumber = v.MobileNumber,
        //                        DefaultLanguageId = 1,
        //                        DefaultLoginTypeId = 20
        //                    };
        //                    c.SubUsers.InsertOnSubmit(_user);
        //                    c.SubmitChanges();

        //                    var _userDetail = new SubUsersDetail()
        //                    {
        //                        UId = _user.UId,
        //                        FullName = v.Name,
        //                        Email = v.Email,
        //                        AMobileNumber = v.AMobileNumber
        //                    };
        //                    c.SubUsersDetails.InsertOnSubmit(_userDetail);
        //                    c.SubmitChanges();

        //                    var _userOrg = new SubUserOrganisation()
        //                    {
        //                        UId = _user.UId,
        //                        OId = _OId.OId,
        //                        RId = _OrgRole.RId
        //                    };
        //                    c.SubUserOrganisations.InsertOnSubmit(_userOrg);
        //                    c.SubmitChanges();
        //                    _subUser = _user;
        //                }

        //                var _users = c.SubUsers.SingleOrDefault(x => x.MobileNumber == v.MobileNumber);
        //                var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _users.UId && x.OId == _OId.OId && x.RId == _OrgRole.RId);

        //                var _Sid = (from y in c.DevOrganisationsStaffs
        //                            where y.OId == _OId.OId
        //                            select y).Max(x => x.SId);
        //                var i = _Sid;
        //                if (_Sid == null)
        //                {
        //                    _Sid = 1;
        //                }
        //                else
        //                {
        //                    _Sid += 1;
        //                }
        //                var id = _Sid;

        //                var staff = new DevOrganisationsStaff()
        //                {
        //                    URId = _URID.URId,
        //                    OId = (int)v.Organization.Id,
        //                    ShiftTimeId = v.ShiftTiming.Id,
        //                    Salary = v.Salary,
        //                    IsOpenWeek = v.IsOpenWeek,
        //                    SId = _Sid,
        //                    Status = false,
        //                    CreateDate = ISDT,
        //                };
        //                if (v.IsOpenWeek == false)
        //                {
        //                    staff.WeekOffOneDay = v.WeekOff1.Id;
        //                    staff.WeekOffSecondDay = v.WeekOff2.Id;
        //                }
        //                c.DevOrganisationsStaffs.InsertOnSubmit(staff);
        //                c.SubmitChanges();
        //            });

        //            scope.Complete();

        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format("Staff Add Successfully!"),                         
        //            };
        //        }
        //    }
        //}

        //public Result JoinOrganizationCreate(Models.Employer.Organization.Staff.JoinOrganizationCreate value,IConfiguration configuration,ITokenServices tokenServices)   
        //{
        //    using (DBContext c = new DBContext())
        //    {
        //        Cores.Common.Claims claims = new Claims(configuration,tokenServices);
        //        using (TransactionScope scope = new TransactionScope())
        //        {
        //            var _User = c.SubUsers.SingleOrDefault(x =>x.MobileNumber == value.MobileNumber);
        //            if (_User is null)
        //            {
        //                throw new ArgumentException("User Does Not Exits!");
        //            }

        //            var _Org = c.DevOrganisations.SingleOrDefault(x => x.OrgCode == value.OrgCode); 
        //            if (_Org is null)
        //            {
        //                throw new ArgumentException("Organisation Not Found!");
        //            }

        //            var _UserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId==_User.UId && x.OId == _Org.OId);
        //            if (_UserOrg is null)
        //            {
        //                throw new ArgumentException("Unauthorized!");
        //            }

        //            var _staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == _UserOrg.URId);
        //            if(_staff is null)
        //            {  
        //                throw new ArgumentException("Unauthorized!");
        //            }

        //            _staff.Status = true;
        //            c.SubmitChanges();
        //            var devicetoken = (from x in c.SubUserTokens where x.UId == _User.UId select x.CommonDeviceToken.DeviceToken).FirstOrDefault();
        //            var res = claims.Add(_User.UId,devicetoken,_UserOrg.URId);
        //            var username = _User.SubUsersDetail.FullName;
        //            scope.Complete();

        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = string.Format("Staff Add Successfully!"),
        //                Data = new
        //                {
        //                    UserName = username,
        //                    JWT = res.JWT,
        //                    RToken=res.RToken,
        //                }
        //            };
        //        }
        //    }
        //}
    }
}
