using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Development.Subscriber
{
    public class UserIdentitys
    {
        public Result Add(Models.Developer.Subscriber.UserIdentity value,string UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var qs = context.SubUserTokens.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (qs == null) 
                    {
                        throw new ArgumentException("User Doesnt exist!");
                    }
                    SubUsersIdentity identity = new SubUsersIdentity();
                    identity.UId = int.Parse(UID);
                    identity.PanNumber = value.PanNumber;
                    identity.PanFileId = value.PanFileId;
                    identity.AadharNumber = value.AadharNumber;
                    identity.AadharFrontFileId = value.AadharFrontFileId;
                    identity.AadharBackFileId = value.AadharBackFileId;
                    context.SubUsersIdentities.InsertOnSubmit(identity);
                    context.SubmitChanges();
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
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var useridentity = context.SubUsersIdentities.Where(x => x.UserIdentityId == value.UserIdentityId).SingleOrDefault();
                    if (useridentity != null) 
                    {
                        context.SubUsersIdentities.DeleteOnSubmit(useridentity);
                        context.SubmitChanges();
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
