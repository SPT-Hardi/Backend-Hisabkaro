using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common.Contact
{
    public class ContactAddress
    {
        internal Result Create(Models.Common.Contact.Address value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _add = c.CommonContactAddresses.SingleOrDefault(x=> x.AddressLine1 == value.AddressLine1 && x.AddressLine2 == value.AddressLine2 && x.City == value.City && x.State == value.State &&x.PinCode == value.PinCode && x.Landmark == value.LandMark);
                    if(_add is null)
                    {
                        var add = new CommonContactAddress()
                        {
                            AddressLine1 = value.AddressLine1,
                            AddressLine2 = value.AddressLine2,
                            City = value.City,
                            State = value.State,
                            PinCode = value.PinCode,
                            Landmark = value.LandMark,
                        };
                        c.CommonContactAddresses.InsertOnSubmit(add);
                        c.SubmitChanges();
                    }
                    var _adds = c.CommonContactAddresses.SingleOrDefault(x => x.AddressLine1 == value.AddressLine1 && x.AddressLine2 == value.AddressLine2 && x.City == value.City && x.State == value.State && x.PinCode == value.PinCode && x.Landmark == value.LandMark);
                    scope.Complete();

                    var id = _adds.ContactAddressId;
                    return new Models.Common.Result
                    {
                        Status = Models.Common.Result.ResultStatus.success,
                        Message = string.Format("Contact Address Added Successfully!"),
                        Data = _adds.ContactAddressId
                    };
                }
            }            
        }
    }
}
