using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class OtherLanguages
    {
        public Result View(object UId) 
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
                    throw new ArgumentException("User not found!");
                }
                var Profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                if (Profile == null) 
                {
                    throw new ArgumentException("User resume not created yet!");
                }
                var OtherLanguages = (from x in c.EmpResumeOtherLanguages
                                      where x.UId == (int)UId && x.ProfileId == Profile.ProfileId
                                      select new Models.Employee.Resume.OtherLanguages()
                                      {
                                          language=x.OtherLanguage
                                      }).ToList();
                return new Result()
                {
                    Status=Result.ResultStatus.success,
                    Message="User other-languages list get successfully!",
                    Data=OtherLanguages
                };
            }
        }
        public Result Update(object UId,Models.Employee.Resume.List_OtherLanguages value)
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
                        throw new ArgumentException("User not found!");
                    }
                    var Profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (Profile == null)
                    {
                        throw new ArgumentException("User resume not created yet!");
                    }
                    if (Profile.EmpResumeOtherLanguages.ToList().Any()) 
                    {
                        c.EmpResumeOtherLanguages.DeleteAllOnSubmit(Profile.EmpResumeOtherLanguages.ToList());
                        c.SubmitChanges();
                    }
                    var OtherLanguages = (from x in value.OtherLanguageList
                                          select new EmpResumeOtherLanguage()
                                          {
                                              ProfileId=Profile.ProfileId,
                                              UId=user.UId,
                                              OtherLanguage=x.language
                                          }).ToList();
                    c.EmpResumeOtherLanguages.InsertAllOnSubmit(OtherLanguages);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User other-languages updated successfully!",
                        Data = value
                    };
                }
            }
        }
    }
}
