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
                    scope.Complete();

                    var id = add.ContactAddressId;
                    return new Models.Common.Result
                    {
                        Status = Models.Common.Result.ResultStatus.success,
                        Message = string.Format("Contact Address Added Successfully!"),
                        Data = add.ContactAddressId
                    };
                }
            }            
        }
    }
}
