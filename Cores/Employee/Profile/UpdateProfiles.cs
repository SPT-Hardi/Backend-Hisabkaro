using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Profile
{
    public class UpdateProfiles
    {
        public Result Update(object UId,int Id,Models.Employee.Resume.PersonalInfo value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("user not exist!");
                    }
                    var profile = (from x in c.EmpResumeProfiles where x.ProfileId == Id && x.UId == user.UId select x).FirstOrDefault();
                    if (profile == null) 
                    {
                        throw new ArgumentException($"no record found for given Id:{Id}");
                    }
                    profile.CurrentSalary = value.CurrentSalary;
                    profile.Email = value.Email;
                    profile.EnglishLevelId = value.EnglishLevel.Id;
                    profile.FullName = value.Name;
                    profile.IsVisibleToBussinessOwner = value.IsVisibleToBusinessOwner;
                    profile.MobileNumber = value.MobileNumber;
                    profile.UId = user.UId;
                    profile.SalaryTypeId = value.SalaryType.Id;
                    profile.AddressId = value.AddressId;
                    c.SubmitChanges();

                    if (profile.EmpResumeOtherLanguages.ToList().Any())
                    {
                        c.EmpResumeOtherLanguages.DeleteAllOnSubmit(profile.EmpResumeOtherLanguages.ToList());
                        c.SubmitChanges();
                    }

                    //create new user_Resume_OtherLanguages
                    c.EmpResumeOtherLanguages.InsertAllOnSubmit(value.OtherLanguages.Where(x => x.language != null).Select(x => new EmpResumeOtherLanguage()
                    {
                        OtherLanguage = x.language,
                        ProfileId = profile.ProfileId,
                        UId = user.UId
                    }));
                    c.SubmitChanges();

                    var res = new
                    {
                        ProfileId=profile.ProfileId,
                        Name=value.Name
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Profile updated successfully!",
                        Data = res
                    };
                }
            }
        }
    }
}
