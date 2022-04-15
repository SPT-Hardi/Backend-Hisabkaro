using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Developer.Subscriber
{
    public class UserWorkExperiences
    {
        //public Result Add(Models.Developer.Subscriber. value,string UID) 
        //{
        //    using (TransactionScope scope = new TransactionScope())
        //    {
        //        using (DBContext c = new DBContext())
        //        {
        //            var user = c.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
        //            if (user == null) 
        //            {
        //                throw new ArgumentException("User doent exist,(enter valid token)");
        //            }
                    
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = "UserWorkExperience saved successfully!",
        //                Data = "",
        //            };
        //        }
        //    }
        //}
    }
}
