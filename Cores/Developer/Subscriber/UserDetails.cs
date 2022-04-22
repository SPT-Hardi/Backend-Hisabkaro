using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Developer.Subscriber
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
        public Result Add(int UID,string DeviceToken,Models.Developer.Subscriber.UserDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {

                    var user = c.SubUsers.Where(x => x.UId == UID).SingleOrDefault();
                    //we need to change this insertion one time only after,change inside app
                    //20 for Raj employee
                    //21 for employer
                    if (user == null)
                    {
                        throw new ArgumentException("User not Exist!");
                    }
                    var usersDetail = c.SubUsersDetails.Where(x => x.UId == UID).SingleOrDefault();
                    if(usersDetail!=null && user.LoginTypeId != null) 
                    {
                        throw new ArgumentException("User details already exist!");
                    }
                    user.LoginTypeId = value.role.Id;
                    c.SubmitChanges();
                    var file = c.CommonFiles.Where(x => x.FGUID == value.userdetails.ProfilePhotoFGUID).SingleOrDefault();
                    SubUsersDetail udetails = new SubUsersDetail();
                    udetails.AMobileNumber = value.userdetails.AMobileNumber;
                    udetails.Email = value.userdetails.Email;
                    udetails.FileId = file==null ? null : file.FileId;
                    udetails.FullName = value.userdetails.FullName;
                    udetails.UId = UID;
                    Claims claims = new Claims(_configuration, _tokenServices);
                    int URId = 0;
                    var res = claims.Add(UID, DeviceToken,URId);
                    c.SubUsersDetails.InsertOnSubmit(udetails);
                    c.SubmitChanges();

                    var role = c.SubFixedLookups.Where(x => x.FixedLookupId == user.LoginTypeId).SingleOrDefault();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Role added for {user.MobileNumber} mobile number",
                        Data = new  
                        {
                            JWT=res.JWT,
                            RToken=res.RToken,
                            RoleId=role.FixedLookupId,
                            RoleName=role.FixedLookup,
                            UserDetailsId=udetails.UId,
                        }
                        
                    };
                }
            }
        }
       
        

    }
}
