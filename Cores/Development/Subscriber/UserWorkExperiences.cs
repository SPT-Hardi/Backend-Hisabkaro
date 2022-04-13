using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Development.Subscriber
{
    public class UserWorkExperiences
    {
        public Result Add(Models.Developer.Subscriber.UserWorkExperience value,string UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var user = context.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doent exist,(enter valid token)");
                    }
                    
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "UserWorkExperience saved successfully!",
                        Data = "",
                    };
                }
            }
        }
    }
}
