using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Developer.Subscriber;
using HIsabKaro.Services;
using HisabKaroContext;
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
        public Result Add(object UId,Models.Developer.Subscriber.UserPersonalDetails value) 
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null) 
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                c.SubUsersDetails.InsertOnSubmit(new SubUsersDetail()
                {
                    AddressID=null,
                    AMobileNumber=value.AMobileNumber,
                    Email=value.Email,
                    FileId=(from x in c.CommonFiles where x.FGUID==value.ProfilePhotoFGUID select x.FileId).FirstOrDefault(),
                    FullName=value.FullName,
                    UId=user.UId,
                });
                c.SubmitChanges();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "user details added successflly!",
                    Data =new 
                    {
                        UId=user.UId,
                        FullName=value.FullName,
                        MobileNumber=user.MobileNumber
                    },
                };
            }
        }
        /*
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
      

        public UserDetails(IConfiguration configuration, ITokenServices tokenServices) 
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
           
        }
        public Result Add(object UID,object DeviceToken,Models.Developer.Subscriber.UserDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    SubRole addrole = new SubRole();
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    //we need to change this insertion one time only after,change inside app
                    //20 for  employee
                    //21 for employer
                    if (user == null)
                    {
                        throw new ArgumentException("User not Exist!");
                    }
                    var usersDetail = c.SubUsersDetails.Where(x => x.UId == (int)UID).SingleOrDefault();
                   
                    if(usersDetail!=null && user.DefaultLoginTypeId != null) 
                    {
                        throw new ArgumentException("User details already exist!");
                    }
                    if (user.MobileNumber == value.userdetails.AMobileNumber) 
                    {
                        throw new ArgumentException("Alternate Mobilenumber and Mobilenumber should be different!");
                    }
                    //update defaultloginid in user table
                    user.DefaultLoginTypeId = value.role.Id;
                    c.SubmitChanges();

                    //add user role in subroles
                  *//*  addrole.RoleName = "Employee";
                    addrole.LoginTypeId = (int)value.role.Id;
                    c.SubRoles.InsertOnSubmit(addrole);
                    c.SubmitChanges();*//*
                    

                    //entry userdeatils
                    var file = c.CommonFiles.Where(x => x.FGUID == value.userdetails.ProfilePhotoFGUID).SingleOrDefault();
                    SubUsersDetail udetails = new SubUsersDetail();
                    udetails.AMobileNumber = value.userdetails.AMobileNumber;
                    udetails.Email = value.userdetails.Email;
                    udetails.FileId = file == null ? null : file.FileId;
                    udetails.FullName = value.userdetails.FullName;
                    udetails.UId = (int)UID;
                    Claims claims = new Claims(_configuration, _tokenServices);
                    int URId = 0;
                    var res = claims.Add(((int)UID), (string)DeviceToken,URId);
                    c.SubUsersDetails.InsertOnSubmit(udetails);
                    c.SubmitChanges();

                    var role = c.SubFixedLookups.Where(x => x.FixedLookupId == user.DefaultLoginTypeId).SingleOrDefault();
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

        public Result Get(object UID) 
        {
            using (DBContext c = new DBContext())
            {
                if (UID == null) 
                {
                    throw new ArgumentException("Token is not valid or expired!");
                }
                var res = (from x in c.SubUsersDetails
                           where x.UId == (int)UID
                           select new UserPersonalDetails()
                           {
                              FullName=x.FullName,
                              Email=x.Email,
                              MobileNumber=x.SubUser.MobileNumber,
                              AMobileNumber=x.AMobileNumber,
                              ProfilePhotoFGUID = x.FileId==null ? null :x.CommonFile.FGUID,
                           }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "UserDetails get successfully!",
                    Data =res,
                };
            }
        }

        public Result Update(object UID, UserPersonalDetails value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    if (UID == null)
                    {
                        throw new ArgumentException("Token is not valid or expired!");
                    }
                    var res = (from x in c.SubUsersDetails
                               where x.UId == (int)UID
                               select x).FirstOrDefault();
                    var file = (from x in c.CommonFiles where x.FGUID == value.ProfilePhotoFGUID select x).FirstOrDefault();
                    res.AMobileNumber = value.AMobileNumber;
                    res.Email = value.Email;
                    res.FullName = value.FullName;
                    res.FileId = file?.FileId;
                    c.SubmitChanges();
                    var MobileNumber = res.SubUser.MobileNumber;

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "UserDetails updated successfully!",
                        Data = new 
                        {
                            FullName=res.FullName,
                            MobileNumber=MobileNumber
                        },
                    };
                }
            }
        }*/

    }
}
