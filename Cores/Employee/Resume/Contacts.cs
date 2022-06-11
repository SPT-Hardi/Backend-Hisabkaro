using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employee.Resume;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Contacts
    {
        public Result Update(object UId,Contact value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var contact = c.SubUsersDetails.Where(x => x.UId == (int)UId).SingleOrDefault();
                    if (UId == null) 
                    {
                        throw new ArgumentException("Token not-valid or expired!");
                    }
                    if (contact == null) 
                    {
                        throw new ArgumentException("There are no any contact details for this Id!");
                    }
                    /*if (value.MobileNumber != contact.SubUser.MobileNumber) 
                    {
                        throw new ArgumentException("You cant change your mobilenumber!");
                    }*/
                    contact.Email = value.Email;
                    contact.AMobileNumber = value.AMobileNumber;
                    
                    
                    ContactAddress contactAddress = new ContactAddress();
                    var res = contactAddress.Create(contact.AddressID,value.Address);
                    contact.AddressID = res.Data;
                    

                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee conatct details updated successfully!",
                        Data = new
                        {
                            UId =UId,
                            FullName =contact.FullName,
                        }
                    };
                }
            }
        }
        public Result Get(object UId) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var contact = c.SubUsersDetails.Where(x => x.UId == (int)UId).SingleOrDefault();
                    if (UId == null)
                    {
                        throw new ArgumentException("Token not-valid or expired!");
                    }
                    if (contact == null) 
                    {
                        throw new ArgumentException("There are no contact details for current user,(enter valid token)");
                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee contact details get successfully!",
                        Data = new
                        {
                            UId =contact.UId,
                            Email =contact.Email,
                            MobileNumber =contact.SubUser.MobileNumber,
                            AMobileNumber =contact.AMobileNumber,
                            AddressId=contact.AddressID,
                            Address =contact.CommonContactAddress?.AddressLine1,
                            City =contact.CommonContactAddress?.City,
                            State =contact.CommonContactAddress?.State,
                            PinCode =contact.CommonContactAddress?.PinCode,
                            LandMark =contact.CommonContactAddress?.Landmark,
                        }
                    };
                }
            }

        }
    }
}
