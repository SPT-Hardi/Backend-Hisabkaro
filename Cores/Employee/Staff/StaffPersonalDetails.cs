﻿using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff
{
    public class StaffPersonalDetails
    {
        private readonly ContactAddress _contactAddress;

        public StaffPersonalDetails(ContactAddress contactAddress)
        {
            _contactAddress = contactAddress;
        }
        public Result One(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var _SId = c.DevOrganisationsStaffs.SingleOrDefault(o => o.URId == URId);
                if (_SId is null)
                {
                    throw new ArgumentException("Satff Does Not Exits!");
                }

                var _Org = (from x in c.DevOrganisationsStaffs
                            where x.URId == URId
                            select new
                            {
                                URId=URId,
                                DOB =x.DOB,
                                Gender =x.Gender,
                                Address =(from y in c.CommonContactAddresses
                                       where y.ContactAddressId == x.SubUserOrganisation.SubUser.SubUsersDetail.AddressID
                                          select new
                                          {
                                              AddressLine1=y.AddressLine1,
                                              AddressLine2=y.AddressLine2,
                                              City=y.City,
                                              State=y.State,
                                              PinCode=y.PinCode,
                                              LandMark=y.Landmark
                                          }).FirstOrDefault(),
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }

        public Result Create(int URId,Models.Employee.Staff.StaffPersonalDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(u => u.URId == URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == URId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    var _AId = _contactAddress.Create(value.Address);
                    _Staff.DOB = value.DOB;
                    _Staff.Gender = value.Gender;
                    _Staff.SubUserOrganisation.SubUser.SubUsersDetail.AddressID = _AId.Data;

                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Personal Detail Add Successfully!"),
                    };
                }
            }
        }
    }
}