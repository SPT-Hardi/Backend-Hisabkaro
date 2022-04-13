using HisaabKaro.Cores.Common;
using HisaabKaro.Models.Common;
using HisaabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Development.Subscriber
{
    public class UserDetails
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
      

        public UserDetails(IConfiguration configuration, ITokenServices tokenServices) 
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
           
        }
        public Result Add(Models.Developer.Subscriber.UserDetails value, string UID,string DeviceToken)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var check = context.SubUsersDetails.Where(x => x.UId.ToString() == UID).SingleOrDefault();

                    if (check != null)
                    {
                        throw new ArgumentException("User Details Already Exist!");
                    }
                    SubUsersDetail udetails = new SubUsersDetail();
                    udetails.AMobileNumber = value.AMobileNumber;
                    udetails.Email = value.Email;
                    udetails.FileId = value.PhotoID==0?1:value.PhotoID;
                    udetails.FullName = value.FullName;
                    udetails.UId = int.Parse(UID);
                    Claims claims = new Claims(_configuration,_tokenServices);
                    var res =claims.Add(UID,DeviceToken);
                    context.SubUsersDetails.InsertOnSubmit(udetails);
                    context.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"{value.FullName}'s Details Saved successful!",
                        Data = new Models.Common.Token()
                        {
                            JWT =res.JWT,
                            RToken =res.RToken,
                        }
                    };
                }
            }
        }
        public Result AddUserProfileImage(Models.Common.File.Upload value,string UID,string DeviceToken, IWebHostEnvironment Environment)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var qs = context.SubUserTokens.Where(x => x.UId.ToString() == UID && x.DeviceToken==DeviceToken).SingleOrDefault();
                    FileUploadServices fileUploadServices = new FileUploadServices(Environment);
                    var res =fileUploadServices.Upload(value);
                    if (qs == null)
                    {
                        throw new ArgumentNullException("User doesnt exist");
                    }
                    else 
                    {
                        var img = context.SubUsersDetails.Where(x => x.UId == qs.UId).SingleOrDefault();
                        if (img == null)
                        {
                            SubUsersDetail subUsersDetail = new SubUsersDetail();
                            subUsersDetail.FileId =res.Data;
                            subUsersDetail.UId = int.Parse(UID);
                            context.SubUsersDetails.InsertOnSubmit(subUsersDetail);
                            context.SubmitChanges();
                        }
                        else 
                        {
                            img.FileId =res.Data;
                            context.SubmitChanges();
                        }
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User profile photo added successfully!",
                        Data = res.Data,
                    };
                }
            }
        }
    }
}
