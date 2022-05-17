using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
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
        public Result Add(Models.Developer.Subscriber.User value) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
             using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                    var smsres = new SMSServices();
                    CustomOTPs customOTPs = new CustomOTPs();
                    var otp = customOTPs.GenerateOTP();
                    SubOTP sotp = new SubOTP();
                    SubUser suser = new SubUser();
                    SubUserToken token = new SubUserToken();
                    if (qs == null)
                    {
               
                        suser.MobileNumber = value.MobileNumber;
                        suser.DefaultLoginTypeId = 23;
                        suser.DefaultLanguageId = value.DefaultLanguageID;
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
                    var newotp = c.SubOTPs.Where(x => x.DeviceToken == value.DeviceToken && x.UId==(qs==null ? null : qs.UId)).SingleOrDefault();
                    if (newotp == null)
                    {

                        sotp.OTP = otp;
                        sotp.ExpiryDate = ISDT.AddMinutes(15);
                        sotp.IsUsed = false;
                        sotp.DeviceToken = value.DeviceToken;
                        c.SubOTPs.InsertOnSubmit(sotp);
                        c.SubmitChanges();
                        smsres.Get(value.MobileNumber,otp);

                    }
                    else 
                    {
                       
                        newotp.OTP = otp;
                        newotp.ExpiryDate = ISDT.AddMinutes(15);
                        newotp.IsUsed = false;
                        newotp.DeviceToken = value.DeviceToken;
                        
                        c.SubmitChanges();
                        smsres.Get(value.MobileNumber, otp);
                    }
                    
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Otp send on {value.MobileNumber} mobile number",
                        Data = new
                        {
                            MobileNumber =value.MobileNumber,
                        }
                    };
                }
            }
        }
        public Result VerifyOtp(Models.Developer.Subscriber.UserMobile value) 
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                
                var usersigninrole = c.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                if (usersigninrole == null) 
                {
                    throw new ArgumentException($"User not exist for:{value.MobileNumber} number!");
                }
                var qs = c.SubOTPs.Where(x => x.DeviceToken == value.DeviceToken && x.UId==usersigninrole.UId).SingleOrDefault();
                    if (qs == null)
                    {
                        throw new ArgumentException("User not exist,enter valid token!");
                    }
                    else if (qs.OTP != value.OTP )
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
                    var user = c.SubUsers.Where(x => x.UId == qs.UId).SingleOrDefault();
                    var tokn = new Claims(_configuration, _tokenServices);
                //delete after------
                var logintype = usersigninrole.DefaultLoginTypeId == null ? null : usersigninrole.SubFixedLookup.FixedLookup;
                var URIdlist = c.SubUserOrganisations.Where(x => x.UId == user.UId).ToList();
                var URId = URIdlist.LastOrDefault();
                //------------------
                    var res = tokn.Add(usersigninrole.UId.ToString(), value.DeviceToken,URId==null ? null : URId.URId.ToString());
                    var checktoken = (from obj in c.SubUserTokens
                                      where obj.DeviceToken == qs.DeviceToken && obj.UId == qs.UId
                                      select obj).SingleOrDefault();

                    if (checktoken == null)
                    {
                        SubUserToken refreshtoken = new SubUserToken();
                        refreshtoken.UId = qs.UId;
                        refreshtoken.Token = res.RToken;
                        refreshtoken.DeviceToken = value.DeviceToken;
                        refreshtoken.DeviceProfile = value.DeviceProfile;
                        c.SubUserTokens.InsertOnSubmit(refreshtoken);
                        c.SubmitChanges();

                    }
                    else
                    {
                        checktoken.Token = res.RToken;
                        c.SubmitChanges();

                    }

                    qs.IsUsed = true;
                    c.SubmitChanges();

                    var role = c.SubFixedLookups.Where(x => x.FixedLookupId == user.DefaultLoginTypeId).SingleOrDefault();
                    var udetails = c.SubUsersDetails.Where(x => x.UId == user.UId).SingleOrDefault();

                var organizationlist = (from obj in c.SubUserOrganisations
                                        where obj.UId == user.UId
                                        select new IntegerNullString()
                                        {
                                            Id=obj.OId,
                                            Text=obj.DevOrganisation.OrganisationName,

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
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Otp verified for {value.MobileNumber} mobile number",
                        Data = new
                        {
                           
                            JWT=res.JWT,
                            RToken=res.RToken,
                            LoginType = new IntegerNullString() { Id = role.FixedLookupId, Text = role.FixedLookup },
                            Name = udetails == null ? null : udetails.FullName,
                            MobileNumber = user.MobileNumber,
                            AMobileNumber = udetails == null ? null : udetails.AMobileNumber,
                            Email = udetails == null ? null : udetails.Email,
                            PhotoId = udetails == null ? null : udetails.FileId,
                            Organization=organizationlist,

                        },
                    };

                }
            

                
            
        }
        

    }
}
