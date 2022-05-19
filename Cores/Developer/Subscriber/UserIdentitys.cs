using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Developer.Subscriber
{
    public class UserIdentitys
    {
        public Result Add(object UID,Models.Developer.Subscriber.UserIdentity value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var qs = c.SubUserTokens.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (qs == null) 
                    {
                        throw new ArgumentException("User Doesnt exist!");
                    }
                    SubUsersIdentity identity = new SubUsersIdentity();
                    identity.UId =(int)UID;
                    identity.PanNumber = value.PanNumber;
                    identity.PanFileId = value.PanFileId;
                    identity.AadharNumber = value.AadharNumber;
                    identity.AadharFrontFileId = value.AadharFrontFileId;
                    identity.AadharBackFileId = value.AadharBackFileId;
                    c.SubUsersIdentities.InsertOnSubmit(identity);
                    c.SubmitChanges();
                    scope.Complete();

                    value.UserIdentityId = identity.UserIdentityId;
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User Identity details saved successfully!",
                        Data = value,
                    };
                }
            }
        }
        public Result Delete(Models.Developer.Subscriber.UserIdentity value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var useridentity = c.SubUsersIdentities.Where(x => x.UserIdentityId == value.UserIdentityId).SingleOrDefault();
                    if (useridentity != null) 
                    {
                        c.SubUsersIdentities.DeleteOnSubmit(useridentity);
                        c.SubmitChanges();
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User Identity details saved successfully!",
                        Data = value,
                    };
                }
                
            }
        }
    }
}
