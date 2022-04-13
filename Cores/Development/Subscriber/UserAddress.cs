using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Development.Subscriber
{
    public class UserAddress
    {
        public Result Add(Models.Common.Address value,string UID,string DeviceToken) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var user = context.SubUsersDetails.Where(x => x.UId.ToString() == UID ).SingleOrDefault();
                    CommonContactAddress addr = new CommonContactAddress();
                    if (user == null)
                    {
                        throw new ArgumentException("User Not Exist!");
                    }
                    else if (user.AddressID == null)
                    {
                        addr.AddressLine1 = value.AddressLine1;
                        addr.AddressLine2 = value.AddressLine2;
                        addr.City = value.City;
                        addr.Landmark = value.Landmark;
                        addr.PinCode = value.PinCode;
                        addr.State = value.State;
                        context.CommonContactAddresses.InsertOnSubmit(addr);
                        context.SubmitChanges();

                        user.AddressID = addr.ContactAddressId;
                        user.CompleteProfile = 1;
                        context.SubmitChanges();
                    }
                    else
                    {
                        var uadr = context.CommonContactAddresses.Where(x => x.ContactAddressId == user.AddressID).SingleOrDefault();
                        uadr.AddressLine1 = value.AddressLine1;
                        uadr.AddressLine2 = value.AddressLine2;
                        uadr.City = value.City;
                        uadr.Landmark = value.Landmark;
                        uadr.PinCode = value.PinCode;
                        uadr.State = value.State;
                        context.SubmitChanges();

                    }

                    scope.Complete();
                 
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Address Details Added successfully!",
                        Data = value,
                    };
                }
            }
        }
    }
}
