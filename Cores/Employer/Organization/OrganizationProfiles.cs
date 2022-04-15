using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationProfiles
    {
        private readonly ContactAddress _contactAddress;
        private readonly ShiftTimes _shiftTimes;

        public OrganizationProfiles(ContactAddress contactAddress,ShiftTimes shiftTimes)
        {
            _contactAddress = contactAddress;
            _shiftTimes = shiftTimes;
        }
        public Result One(int OId)
        {
            using (DBContext c = new DBContext())
            {
                var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == OId);
                if (_OId is null)
                {
                    throw new ArgumentException("Organization Does Not Exits!");
                }

                var _Org = (from x in c.DevOrganisations
                         where x.OId == OId
                         select new
                         {
                             Organization = new IntegerNullString { ID = x.OId, Text = x.OrganisationName },
                             Image = (from f in c.CommonFiles
                                      where f.FileId == x.LogoFileId
                                      select f.FilePath).SingleOrDefault(),
                             GSTNumber = x.GSTIN,
                             GST = (from f in c.CommonFiles
                                    where f.FileId == x.GSTFileId
                                    select f.FilePath).SingleOrDefault(),
                             PanCardNumber = x.PAN,
                             PanCard = (from f in c.CommonFiles
                                        where f.FileId == x.PANFileId
                                        select f.FilePath).SingleOrDefault(),
                             Email = x.Email,
                             MobileNumber = x.MobileNumber,
                             Partners = (from p in c.DevOrganisationsPartners
                                         where p.OId == x.OId
                                         select new Models.Employer.Organization.Partner
                                         {
                                             Email = p.Email,
                                             Mobilenumber = p.MobleNumber,
                                             OwnershipTypeID = (from l in c.SubLookups
                                                                where l.LookupId == p.OwnershipTypeId
                                                                select new IntegerNullString { ID = l.LookupId, Text = l.Lookup }).SingleOrDefault()
                                         }).ToList()
                         }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }
        public Result Create(int UserID,Models.Employer.Organization.OrganizationProfile value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope=new TransactionScope())
                {
                    var _UserID = c.SubUsers.Where(u => u.UId == UserID).SingleOrDefault();
                    if (_UserID is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.ID);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    if (!value.Partners.Any())
                    {
                        throw new ArgumentException("Enter Partner Details !");
                    }

                    var listPartner = value.Partners.Select(x => new { Email = x.Email, MobileNumber = x.Mobilenumber }).ToList();
                    if (listPartner.Distinct().Count() != listPartner.Count())
                    {
                        throw new ArgumentException($"Duplecate Entry In Partners!");
                    }

                    var shifttime = _shiftTimes.Create(_OId.OId, value.ShiftTime);
                    var _AId = _contactAddress.Create(value.Address);

                    _OId.GSTFileId = value.GST;
                    _OId.GSTIN = value.GSTNumber;
                    _OId.ContactAddressId = _AId.Data;
                    _OId.PAN = value.PanCardNumber;
                    _OId.PANFileId = value.PanCard;
                    _OId.Email = value.Email;
                    _OId.MobileNumber = value.MobileNumber;
                    
                    c.SubmitChanges();

                    c.DevOrganisationsPartners.InsertAllOnSubmit(value.Partners.Select(x => new DevOrganisationsPartner()
                    {
                        OId = value.Organization.ID,
                        OwnershipTypeId=x.OwnershipTypeID.ID,
                        Email=x.Email,
                        MobleNumber=x.Mobilenumber,
                    }).ToList());
                    c.SubmitChanges();
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organization Add Successfully"),
                        Data = new { Id = _OId.OId }
                    };
                }
            }
        }

        public Result Update(Models.Employer.Organization.OrganizationProfile value, int UserID)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _UserID = c.SubUsers.Where(u => u.UId == UserID).SingleOrDefault();
                    if (_UserID is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == value.Organization.ID);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    if (!value.Partners.Any())
                    {
                        throw new ArgumentException("Enter Silai Staff Details !");
                    }
                    var list = value.Partners.Select(x => new { Email = x.Email, MobileNumber = x.Mobilenumber }).ToList();
                    if (list.Distinct().Count() != list.Count())
                    {
                        throw new ArgumentException($"Duplecate Entry In Partners!");
                    }

                    var _AId = _contactAddress.Create(value.Address);

                    _OId.GSTFileId = value.GST;
                    _OId.GSTIN = value.GSTNumber;
                    _OId.ContactAddressId = _AId.Data;
                    _OId.PAN = value.PanCardNumber;
                    _OId.PANFileId = value.PanCard;
                    _OId.Email = value.Email;
                    _OId.MobileNumber = value.MobileNumber;

                    c.SubmitChanges();

                    c.DevOrganisationsPartners.InsertAllOnSubmit(value.Partners.Select(x => new DevOrganisationsPartner()
                    {
                        OId = value.Organization.ID,
                        OwnershipTypeId = x.OwnershipTypeID.ID,
                        Email = x.Email,
                        MobleNumber = x.Mobilenumber,
                    }).ToList());
                    c.SubmitChanges();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organization Add Successfully"),
                        Data = _OId.OId
                    };
                }
            }
        }
    }
}
