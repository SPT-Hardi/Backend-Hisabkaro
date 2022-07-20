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
        public Result Create(object Id, Models.Common.Contact.Address value)
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
        public Result GetOrgAddress(object UId, int Id)
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var org = (from x in c.DevOrganisations
                           where x.OId == Id
                           select new
                           {
                               AddressId = x.CommonContactAddress.ContactAddressId,
                               AddressLine1 = x.CommonContactAddress.AddressLine1,
                               AddressLine2 = "",
                               City = x.CommonContactAddress.City,
                               LandMark = x.CommonContactAddress.Landmark,
                               PinCode = x.CommonContactAddress.PinCode,
                               State = x.CommonContactAddress.State,
                               MobileNumber = x.MobileNumber,
                               Email = x.Email,
                           }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Address details get successfully!",
                    Data = org,
                };
            }
        }

        public Result GetBranchAddress(object UId, int Id)
        {
            using (DBContext c = new DBContext())
            {
                if (UId == null)
                {
                    throw new ArgumentException("token not found or expired!");
                }
                var branch = (from x in c.DevOrganisationBranches
                              where x.BranchId == Id
                              select new
                              {
                                  AddressId = x.CommonContactAddress.ContactAddressId,
                                  AddressLine1 = x.CommonContactAddress.AddressLine1,
                                  AddressLine2 = "",
                                  City = x.CommonContactAddress.City,
                                  LandMark = x.CommonContactAddress.Landmark,
                                  PinCode = x.CommonContactAddress.PinCode,
                                  State = x.CommonContactAddress.State,
                                  MobileNumber = x.DevOrganisation.MobileNumber,
                                  Email = x.DevOrganisation.Email,
                              }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Address details get successfully!",
                    Data = branch,
                };
            }
        }
        public Result Get(int Id) 
        {
            using (DBContext c = new DBContext())
            {
                var address = (from x in c.CommonContactAddresses
                              where x.ContactAddressId == Id
                              select new HIsabKaro.Models.Common.Contact.Address()
                              {
                                  AddressId=x.ContactAddressId,
                                  AddressLine1 = x.AddressLine1,
                                  AddressLine2 = "",
                                  City = x.City,
                                  LandMark = x.Landmark,
                                  PinCode = x.PinCode,
                                  State = x.State,
                              }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Address details get successfully!",
                    Data = address,
                };
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
