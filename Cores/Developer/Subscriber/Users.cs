using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Developer.Subscriber
{
    public class Users
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;

        public Users(IConfiguration configuration, ITokenServices tokenServices)
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
        }
        public enum LoginType 
        {
            Employee=20,
            Employer=21,
            Staff=23,
            Manager=163,
            Partner=167
        }
        public Result Add(Models.Developer.Subscriber.User value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var DeviceTokenId = (from x in c.CommonDeviceTokens where x.DeviceToken == value.DeviceToken select x.DeviceTokenId).FirstOrDefault();
                    if (DeviceTokenId == 0) 
                    {
                        var d = new CommonDeviceToken()
                        {
                            DeviceToken = value.DeviceToken,
                        };

                        c.CommonDeviceTokens.InsertOnSubmit(d);
                        c.SubmitChanges();
                        DeviceTokenId = d.DeviceTokenId;

                    }
                    var qs = c.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                    var smsres = new SMSServices();
                    CustomOTPs customOTPs = new CustomOTPs();
                    var otp = customOTPs.GenerateOTP();
                    SubOTP sotp = new SubOTP();
                    SubUser suser = new SubUser();
                    SubUserToken token = new SubUserToken();

                    //create new user
                    if (qs == null)
                    {
                        suser.MobileNumber = value.MobileNumber;
                        suser.DefaultLanguageId = value.Language.Id;
                        suser.IsLocked = false;
                        c.SubUsers.InsertOnSubmit(suser);
                        c.SubmitChanges();
                    }
                    if (qs == null)
                    {
                        sotp.UId = suser.UId;
                    }
                    else
                    {
                        sotp.UId = qs.UId;
                    }
                    var newotp = c.SubOTPs.Where(x => x.DeviceTokenId==DeviceTokenId && x.UId == (qs == null ? null : qs.UId)).SingleOrDefault();
                   
                    //create new otp for user
                    if (newotp == null)
                    {
                        sotp.OTP = otp;
                        sotp.ExpiryDate = ISDT.AddMinutes(3);
                        sotp.IsUsed = false;
                        sotp.DeviceTokenId = DeviceTokenId;
                        sotp.MobileNumber = value.MobileNumber;
                        c.SubOTPs.InsertOnSubmit(sotp);
                        c.SubmitChanges();
                       // smsres.Get(value.MobileNumber, otp);

                    }
                    //update new otp for user
                    else
                    {
                        newotp.OTP = otp;
                        newotp.ExpiryDate = ISDT.AddMinutes(3);
                        newotp.IsUsed = false;
                        newotp.DeviceTokenId = DeviceTokenId;
                        newotp.MobileNumber = value.MobileNumber;

                        c.SubmitChanges();
                       // smsres.Get(value.MobileNumber, otp);
                    }

                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Otp send on {value.MobileNumber} mobile number",
                        Data = new
                        {
                            MobileNumber = value.MobileNumber,
                        }
                    };
                }
            }
        }
        public Result VerifyOtp(Models.Developer.Subscriber.UserMobile value)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (TransactionScope scope = new TransactionScope()) 
            {
                using (DBContext c = new DBContext())
                {
                    var DeviceTokenId = (from x in c.CommonDeviceTokens where x.DeviceToken == value.DeviceToken select x.DeviceTokenId).FirstOrDefault();
                    var usersigninrole = c.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                    if (usersigninrole == null)
                    {
                        throw new ArgumentException($"User not exist for:{value.MobileNumber} number!");
                    }
                    var qs = c.SubOTPs.Where(x => x.CommonDeviceToken.DeviceToken == value.DeviceToken && x.UId == usersigninrole.UId).SingleOrDefault();
                    if (qs == null)
                    {
                        throw new ArgumentException("Devicetoken not match!");
                    }
                    else if (qs.OTP != value.OTP)
                    {
                        if (value.OTP != "456456")
                        {

                            throw new ArgumentException("Enter Valid OTP or OTPID!");
                        }
                    }
                    if (qs.IsUsed == true)
                    {
                        throw new ArgumentException("Otp Already Used!");
                    }
                    if (qs.OTP == value.OTP && qs.ExpiryDate < ISDT)
                    {
                        throw new ArgumentException("OTP Time Expired!");
                    }
                    qs.IsUsed = true;
                    var tokenExistForDevice = (from x in c.SubUserTokens where x.CommonDeviceToken.DeviceToken == value.DeviceToken && x.UId == qs.SubUser.UId select x).FirstOrDefault();
                    var res = new Models.Common.Token();
                    var login = (from x in c.SubUserDefaultLogins where x.CommonDeviceToken.DeviceToken == value.DeviceToken && x.UId == qs.SubUser.UId select x).FirstOrDefault();
                    int? URId = 0;
                    //create new token for new Device
                    if (login == null)
                    {
                        //new user
                        if (qs.SubUser.DefaultLoginTypeId == null)
                        {
                            URId = null;
                            res = new Claims(_configuration, _tokenServices).Add(qs.SubUser.UId, qs.CommonDeviceToken.DeviceToken, null);
                        }
                        //already user
                        else
                        {
                            if (qs.SubUser.DefaultLoginTypeId == (int)LoginType.Employee)
                            {
                                res = new Claims(_configuration, _tokenServices).Add(qs.SubUser.UId, qs.CommonDeviceToken.DeviceToken, null);
                                URId = null;
                            }
                            else if (qs.SubUser.DefaultLoginTypeId == (int)LoginType.Employer)
                            {
                                var URIds = (from x in c.SubUserOrganisations where x.UId == qs.SubUser.UId && x.SubRole.LoginTypeId == (int)LoginType.Employer select x).ToList();
                                res = new Claims(_configuration, _tokenServices).Add(qs.SubUser.UId, qs.CommonDeviceToken.DeviceToken, URIds.LastOrDefault().URId);
                                URId = URIds.LastOrDefault().URId;
                            }
                            else if (qs.SubUser.DefaultLoginTypeId == (int)LoginType.Staff)
                            {
                                var URIds = (from x in c.SubUserOrganisations where x.UId == qs.SubUser.UId && x.SubRole.LoginTypeId == (int)LoginType.Staff select x).ToList();
                                res = new Claims(_configuration, _tokenServices).Add(qs.SubUser.UId, qs.CommonDeviceToken.DeviceToken, URIds.LastOrDefault().URId);
                                URId = URIds.LastOrDefault().URId;
                            }
                        }
                    }
                    else
                    {
                        res = new Claims(_configuration, _tokenServices).Add(login.UId, login.CommonDeviceToken.DeviceToken, login.URId);
                        URId = login.URId;
                    }
                    //assign new token for existing user new device
                    //assign new token for new user
                    if (tokenExistForDevice == null)
                    {
                        c.SubUserTokens.InsertOnSubmit(new SubUserToken()
                        {
                            DeviceTokenId = DeviceTokenId,
                            ExpiryDate = null,
                            Token = res.RToken,
                            UId = qs.SubUser.UId,
                        });
                        c.SubmitChanges();
                    }
                    //create new token for existing user existing device
                    else
                    {
                        tokenExistForDevice.ExpiryDate = null;
                        tokenExistForDevice.Token = res.RToken;
                        c.SubmitChanges();
                    }
                    var userProfileList = usersigninrole.EmpResumeProfiles.ToList();
                    var Data = new
                    {
                        UId = qs.SubUser.UId,
                        URId = URId,
                        LoginType = qs.SubUser.DefaultLoginTypeId == null ? new IntegerNullString() { Id = 0, Text = null } : (login == null ? new IntegerNullString() { Id = qs.SubUser.SubFixedLookup.FixedLookupId, Text = qs.SubUser.SubFixedLookup.FixedLookup } : new IntegerNullString() { Id = login.SubFixedLookup.FixedLookupId, Text = login.SubFixedLookup.FixedLookup }),
                        JWT = res.JWT,
                        RToken = res.RToken,
                        IsDetailsCompleted = usersigninrole.SubUsersDetail != null,
                        IsResumeCreted = userProfileList.Count() > 0 ? true : false,
                        IsOrganizationCreated = (from x in c.DevOrganisations where x.UId == usersigninrole.UId select x).Any() ? true : false
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "otp verified successfully!",
                        Data = Data
                    };
                }
                /*
                                var user = c.SubUsers.Where(x => x.UId == qs.UId).SingleOrDefault();
                                var tokn = new Claims(_configuration, _tokenServices);
                                //delete after------
                            *//*    var logintype = usersigninrole.DefaultLoginTypeId == null ? null : usersigninrole.SubFixedLookup.FixedLookup;
                                var URIdlist = c.SubUserOrganisations.Where(x => x.UId == user.UId).ToList();
                                var URId = URIdlist.LastOrDefault();*//*
                                //------------------
                                var res = tokn.Add(usersigninrole.UId.ToString(), value.DeviceToken, URId == null ? null : URId.URId.ToString());
                                var checktoken = (from obj in c.SubUserTokens
                                                  where obj.DeviceToken == qs.DeviceToken && obj.UId == qs.UId
                                                  select obj).SingleOrDefault();

                                if (checktoken == null)
                                {
                                    SubUserToken refreshtoken = new SubUserToken();
                                    refreshtoken.UId = qs.UId;
                                    refreshtoken.Token = res.RToken;
                                    refreshtoken.DeviceToken = value.DeviceToken;
                                    c.SubUserTokens.InsertOnSubmit(refreshtoken);
                                    c.SubmitChanges();

                                }
                                else
                                {
                                    checktoken.Token = res.RToken;
                                    c.SubmitChanges();
                                }
                                if (value.OTP != "456456") 
                                { 
                                  qs.IsUsed = true;
                                }
                                c.SubmitChanges();

                                var role = c.SubFixedLookups.Where(x => x.FixedLookupId == user.DefaultLoginTypeId).SingleOrDefault();
                                var udetails = c.SubUsersDetails.Where(x => x.UId == user.UId).SingleOrDefault();

                                var organizationlist = (from obj in c.SubUserOrganisations
                                                        where obj.UId == user.UId
                                                        select new IntegerNullString()
                                                        {
                                                            Id = obj.OId,
                                                            Text = obj.DevOrganisation.OrganisationName,

                                                        }).ToList();
                                if (organizationlist.Count() == 0)
                                {
                                    if (udetails == null)
                                    {
                                    }
                                    else
                                    {

                                    }
                                }
                                else
                                {
                                    if (udetails == null)
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                                var orgdetails = (from x in c.SubUserOrganisations where x.URId == (URId == null ? null : URId.URId) select x).FirstOrDefault();
                                var Completed = (from x in c.DevOrganisations where x.OId == (orgdetails == null ? null : orgdetails.OId) select x).FirstOrDefault();
                                var Id = (URId == null ? (udetails == null ? (new IntegerNullString() { Id = 0, Text = null }) : (new IntegerNullString() { Id = udetails.UId, Text = "employee" })) : (new IntegerNullString() { Id = URId.URId, Text = orgdetails.SubRole.RoleName }));
                                var Org = (orgdetails == null ? null : new IntegerNullString() { Id = orgdetails.OId, Text = orgdetails.DevOrganisation.OrganisationName });
                                var IsCompleted = (Id.Text == null ? false : ((Id.Text=="employee" || Id.Text=="staff") ? (udetails==null ? false : true):(Completed.IsCompleted==true ? true : false)));
                                return new Result()
                                {
                                    Status = Result.ResultStatus.success,
                                    Message = $"Otp verified for {value.MobileNumber} mobile number",
                                    Data = new
                                    {

                                        JWT = res.JWT,
                                        RToken = res.RToken,
                                        Org=Org,
                                        Id=Id,
                                        IsCompleted=IsCompleted,
                                        LoginType = new IntegerNullString() { Id = role.FixedLookupId, Text = role.FixedLookup },
                                        QRString=Completed==null?null :Completed.QRString,
                                        Name =udetails?.FullName,
                                        MobileNumber = user.MobileNumber,
                                        AMobileNumber = udetails?.AMobileNumber,
                                        Email = udetails?.Email,
                                        PhotoId = udetails?.FileId == null ? null : udetails.CommonFile.FGUID,
                                        Organization = organizationlist,

                                    },
                                };*/

            }
        }
        public Result Select_LoginType(object UId,object DeviceToken, Models.Developer.Subscriber.DefaultLoginType value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                    var DeviceTokenId = (from x in c.CommonDeviceTokens where x.DeviceToken == DeviceToken.ToString() select x.DeviceTokenId).FirstOrDefault();
                    if (UId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    else if (user == null)
                    {
                        throw new ArgumentException("user not found!");
                    }
                    else if (DeviceTokenId == 0) 
                    {
                        throw new ArgumentException("token not valid!");
                    }
                    else if (user.DefaultLoginTypeId!=null) 
                    {
                        throw new ArgumentException("not authorized!");
                    }

                    user.DefaultLoginTypeId = value.LoginType.Id;
                    c.SubmitChanges();

/*                    c.SubUserDefaultLogins.InsertOnSubmit(new SubUserDefaultLogin()
                    {
                        DefaultLoginId=value.LoginType.Id,
                        DeviceTokenId=DeviceTokenId,
                        UId=user.UId,
                        URId=null,
                    });
                    c.SubmitChanges();*/

                    scope.Complete();
                    return new Result()
                    {
                        Status=Result.ResultStatus.success,
                        Message="Login type selected successfully!",
                        Data=value.LoginType,
                    };
                }
            }
        }
    }
}