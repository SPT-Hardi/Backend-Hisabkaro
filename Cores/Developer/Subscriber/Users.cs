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
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var qs = context.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                    
                    CustomOTPs customOTPs = new CustomOTPs();
                    var otp = customOTPs.GenerateOTP();
                    SubOTP sotp = new SubOTP();
                    SubUser suser = new SubUser();
                    SubUserToken token = new SubUserToken();
                    if (qs == null)
                    {
               
                        suser.MobileNumber = value.MobileNumber;
                        suser.LoginTypeId = 23;
                        suser.DefaultLanguageId = value.DefaultLanguageID;
                        context.SubUsers.InsertOnSubmit(suser);
                        context.SubmitChanges();

                    
                    }
                    if (qs == null)
                    {
                        sotp.UId = suser.UId;
                    }
                    else 
                    {
                        sotp.UId = qs.UId;
                    }
                    var newotp = context.SubOTPs.Where(x => x.DeviceToken == value.DeviceToken && x.UId==qs.UId).SingleOrDefault();
                    if (newotp == null)
                    {
                        sotp.OTP = "456456";
                        sotp.ExpiryDate = DateTime.Now.AddMinutes(15);
                        sotp.IsUsed = false;
                        sotp.DeviceToken = value.DeviceToken;
                        context.SubOTPs.InsertOnSubmit(sotp);
                        context.SubmitChanges();

                    }
                    else 
                    {
                        newotp.OTP = "456456";
                        newotp.ExpiryDate = DateTime.Now.AddMinutes(15);
                        newotp.IsUsed = false;
                        newotp.DeviceToken = value.DeviceToken;
                        
                        context.SubmitChanges();
                    }
                    


                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Otp send on {value.MobileNumber} mobile number",
                        Data = new Models.Developer.Subscriber.UserMobile()
                        {
                            MobileNumber =value.MobileNumber,
                            OTP="456456",
                            

                        }
                    };
                }
            }
        }
        public Result VerifyOtp(Models.Developer.Subscriber.UserMobile value) 
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {

                var usersigninrole = context.SubUsers.Where(x => x.MobileNumber == value.MobileNumber).SingleOrDefault();
                if (usersigninrole == null) 
                {
                    throw new ArgumentException($"User not exist for:{value.MobileNumber} number!");
                }
                var qs = context.SubOTPs.Where(x => x.DeviceToken == value.DeviceToken && x.UId==usersigninrole.UId).SingleOrDefault();
                    if (qs == null)
                    {
                        throw new ArgumentException("User not exist,enter valid token!");
                    }
                    else if (qs.OTP != value.OTP)
                    {
                        throw new ArgumentException("Enter Valid OTP or OTPID!");
                    }
                    else if (qs.OTP == value.OTP && qs.IsUsed == true)
                    {
                        throw new ArgumentException("Otp Already Used!");
                    }
                    else if (qs.OTP == value.OTP && qs.ExpiryDate < DateTime.Now)
                    {
                        throw new ArgumentException("OTP Time Expired!");
                    }
                    var user = context.SubUsers.Where(x => x.UId == qs.UId).SingleOrDefault();
                    var tokn = new Claims(_configuration, _tokenServices);
                    var res = tokn.Add(usersigninrole.UId.ToString(), value.DeviceToken,usersigninrole.LoginTypeId.ToString());
                    var checktoken = (from obj in context.SubUserTokens
                                      where obj.DeviceToken == qs.DeviceToken && obj.UId == qs.UId
                                      select obj).SingleOrDefault();

                    if (checktoken == null)
                    {
                        SubUserToken refreshtoken = new SubUserToken();
                        refreshtoken.UId = qs.UId;
                        refreshtoken.Token = res.RToken;
                        refreshtoken.DeviceToken = value.DeviceToken;
                        refreshtoken.DeviceProfile = value.DeviceProfile;
                        context.SubUserTokens.InsertOnSubmit(refreshtoken);
                        context.SubmitChanges();

                    }
                    else
                    {
                        checktoken.Token = res.RToken;
                        context.SubmitChanges();

                    }

                    qs.IsUsed = true;
                    context.SubmitChanges();
                    var role = context.SubFixedLookups.Where(x => x.FixedLookupId == user.LoginTypeId).SingleOrDefault();
                    var udetails = context.SubUsersDetails.Where(x => x.UId == user.UId).SingleOrDefault();

                    
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Otp verified for {value.MobileNumber} mobile number",
                        Data = new
                        {
                           
                            JWT=res.JWT,
                            RToken=res.RToken,
                            LoginTypeID = new IntegerNullString() { ID = role.FixedLookupId, Text = role.FixedLookup },
                            Name = udetails == null ? null : udetails.FullName,
                            MobileNumber = user.MobileNumber,
                            AMobileNumber = udetails == null ? null : udetails.AMobileNumber,
                            Email = udetails == null ? null : udetails.Email,
                            PhotoId = udetails == null ? null : udetails.FileId,

                        },
                    };

                }
            

                
            
        }
        

    }
}
