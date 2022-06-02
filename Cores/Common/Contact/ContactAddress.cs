using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common.Contact
{
    public class ContactAddress
    {
        public Result Create(object Id,Models.Common.Contact.Address value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    
                    var address = c.CommonContactAddresses.Where(x => x.ContactAddressId == (int)Id).SingleOrDefault();
                    if (address != null)
                    {
                        address.AddressLine1 = value.AddressLine1;
                        address.AddressLine2 = value.AddressLine2;
                        address.City = value.City;
                        address.State = value.State;
                        address.PinCode = value.PinCode;
                        address.Landmark = value.LandMark;

                        c.SubmitChanges();

                        scope.Complete();

                        return new Models.Common.Result
                        {
                            Status = Models.Common.Result.ResultStatus.success,
                            Message = string.Format("Contact Address Updated Successfully!"),
                            Data = address.ContactAddressId,
                        };
                    }
                    else
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

                        return new Models.Common.Result
                        {
                            Status = Models.Common.Result.ResultStatus.success,
                            Message = string.Format("Contact Address Added Successfully!"),
                            Data = add.ContactAddressId,
                        };
                    }
                    
                }
            }            
        }
/*        public Result Update(int Id, Models.Common.Contact.Address value)  
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var address = c.CommonContactAddresses.Where(x => x.ContactAddressId == Id).SingleOrDefault();
                    if (address == null) 
                    {
                        throw new ArgumentException("There are no any details for current Id!");
                    }
                    address.AddressLine1 = value.AddressLine1;
                    address.AddressLine2 = value.AddressLine2;
                    address.City = value.City;
                    address.State = value.State;
                    address.PinCode = value.PinCode;
                    address.Landmark = value.LandMark;

                    c.SubmitChanges();

                    scope.Complete();
                    return new Models.Common.Result
                    {
                        Status = Models.Common.Result.ResultStatus.success,
                        Message = string.Format("Contact address updated successfully!"),
                        Data = address.ContactAddressId,
                    };
                }
            }
        }*/
    }
}
