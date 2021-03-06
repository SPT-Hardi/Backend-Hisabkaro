
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Developer.Subscriber;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Developer.Subscriber
{
    public class UserCompleteProfiles
    {
        public Result Add(string UID,UserCompleteProfile value )
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var qs = context.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (qs == null)
                    {
                        throw new ArgumentException("User Doesnt exist!");
                    }
                    //------user address---------------------//
                    var userdetail = context.SubUsersDetails.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (userdetail.AddressID == null)
                    {
                        CommonContactAddress contactaddress = new CommonContactAddress()
                        {
                            AddressLine1=value.address.AddressLine1,
                            AddressLine2= value.address.AddressLine2,
                            State= value.address.State,
                            City= value.address.City,
                            PinCode= value.address.PinCode,
                            Landmark= value.address.LandMark,
                            
                        };
                        context.CommonContactAddresses.InsertOnSubmit(contactaddress);
                        context.SubmitChanges();
                        userdetail.AddressID = contactaddress.ContactAddressId;
                        context.SubmitChanges();
                    }
                    else 
                    {
                        throw new ArgumentException("User profile already completed!");
                    }

                    //--------user TotalWorkExperience------------//
                    SubUsersTotalworkexperience totalworkexperience = new SubUsersTotalworkexperience()
                    {
                        UId = int.Parse(UID),
                        Duration = value.totalWorkExperience.Duration.Year + " year" + value.totalWorkExperience.Duration.Month + " month",
                        Description = value.totalWorkExperience.Description,
                        LookingForJob = value.totalWorkExperience.LookingForJob,
                        CurrentlyWorking=value.totalWorkExperience.CurrentlyWorking,
                    };
                    context.SubUsersTotalworkexperiences.InsertOnSubmit(totalworkexperience);
                    context.SubmitChanges();

                    

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User profile-details saved successfully!",
                        Data = new 
                        {
                            AddressId=userdetail.AddressID,
                            Address=value.address.AddressLine1,
                            WorkExperienceId=totalworkexperience.UserTotalWorkExperienceId,
                            Duration=totalworkexperience.Duration,
                        }
                    };
                }
            }
        }
    }
}
