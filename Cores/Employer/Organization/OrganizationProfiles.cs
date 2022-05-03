using CsvHelper;
using HIsabKaro.Cores.Common.Context;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationProfiles
    {
        private readonly ITokenServices _tokenService;
        private readonly ContactAddress _contactAddress;
        private readonly ShiftTimes _shiftTimes;

        public OrganizationProfiles(ITokenServices tokenService,ContactAddress contactAddress,ShiftTimes shiftTimes)
        {
            _tokenService = tokenService;
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
                            select new Models.Employer.Organization.OrganizationProfile
                            {

                                LogoFile = (from f in c.CommonFiles
                                            where f.FileId == x.LogoFileId
                                            select f.FGUID).SingleOrDefault(),
                                GSTNumber = x.GSTIN,
                                GST = (from f in c.CommonFiles
                                       where f.FileId == x.GSTFileId
                                       select f.FGUID).SingleOrDefault(),
                                ShiftTime = (from s in c.DevOrganisationsShiftTimes
                                             where s.OId == x.OId
                                             select new Models.Common.Shift.ShitTime
                                             {
                                                 ShiftTimeId = s.ShiftTimeId,
                                                 StartTime = s.StartTime,
                                                 EndTime = s.EndTime,
                                                 MarkLate = s.MarkLate
                                             }).ToList(),
                                Address = (from a in c.CommonContactAddresses
                                           where a.ContactAddressId == x.ContactAddressId
                                           select new Models.Common.Contact.Address
                                           {
                                               AddressLine1 = a.AddressLine1,
                                               AddressLine2 = a.AddressLine2,
                                               City = a.City,
                                               State = a.State,
                                               PinCode = (int)a.PinCode,
                                               LandMark = a.Landmark
                                           }).SingleOrDefault(),
                                PanCardNumber = x.PAN,
                                PanCard = (from f in c.CommonFiles
                                           where f.FileId == x.PANFileId
                                           select f.FGUID).SingleOrDefault(),
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
                                                                   select new IntegerNullString { Id = l.LookupId, Text = l.Lookup }).SingleOrDefault()
                                            }).ToList()
                            }).FirstOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }
        public Result Create(object UserId,int OId,Models.Employer.Organization.OrganizationProfile value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope=new TransactionScope())
                {
                    var _User = c.SubUsers.Where(u => u.UId == (int)UserId).SingleOrDefault();
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == OId);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    var _UserOrg = c.SubUserOrganisations.SingleOrDefault(x => x.UId == _User.UId && x.OId == _OId.OId);
                    if (_UserOrg is null)
                    {
                        throw new ArgumentException("Unauthorized!");
                    }

                    var listPartner = value.Partners.Select(x => new { Email = x.Email, MobileNumber = x.Mobilenumber }).ToList();
                    if (listPartner.Distinct().Count() != listPartner.Count())
                    {
                        throw new ArgumentException($"Duplecate Entry In Partners!");
                    }

                    if(_OId.ContactAddressId is null)
                    {
                        var _AId = _contactAddress.Create(value.Address);
                        _OId.ContactAddressId = _AId.Data;
                    }
                    else
                    {
                        var _AId = _contactAddress.Update((int)_OId.ContactAddressId,value.Address);
                        _OId.ContactAddressId = _AId.Data;
                    }
                    var shifttime = _shiftTimes.Create(_OId.OId, value.ShiftTime);
                    
                    var _LogoFileId = (from x in c.CommonFiles where x.FGUID == value.LogoFile select x).FirstOrDefault();
                    var _GSTFileId = (from x in c.CommonFiles where x.FGUID == value.GST select x).FirstOrDefault();
                    var _PANFileId = (from x in c.CommonFiles where x.FGUID == value.PanCard select x).FirstOrDefault();

                    _OId.LogoFileId = _LogoFileId == null ? null : _LogoFileId.FileId;
                    _OId.GSTFileId = _GSTFileId==null ? null : _GSTFileId.FileId;
                    _OId.GSTIN = value.GSTNumber;
                    _OId.PAN = value.PanCardNumber;
                    _OId.PANFileId = _PANFileId.FileId;
                    _OId.Email = value.Email;
                    _OId.MobileNumber = value.MobileNumber;
                    
                    c.SubmitChanges();

                    var _Partner = PartnerCreate(_OId.OId, value.Partners);

                    var _OrgRole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId == OId);
                    var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == (int)UserId && x.OId == OId && x.RId == _OrgRole.RId);
                    var authclaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role,_URID.URId.ToString()),
                        new Claim(ClaimTypes.Sid,UserId.ToString()),
                        new Claim(ClaimTypes.Name,_User.SubUserTokens.Select(x=>x.DeviceToken).FirstOrDefault()),
                        new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                    };
                    var jwtToken = _tokenService.GenerateAccessToken(authclaims);

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organization Add Successfully"),
                        Data = new {
                            OId = _OId.OId,
                            JWT = jwtToken,
                        }
                    };
                }
            }
        }

        internal Result PartnerCreate(int OId, List<Models.Employer.Organization.Partner> value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var listPartner = value.Select(x => new { Email = x.Email, MobileNumber = x.Mobilenumber }).ToList();
                    if (listPartner.Distinct().Count() != listPartner.Count())
                    {
                        throw new ArgumentException($"Duplecate Entry In Partners!");
                    }

                    value.ForEach((x) =>
                    {
                        if (x.PartnerId is null)
                        {
                            var partner = new DevOrganisationsPartner()
                            {
                                Email=x.Email,
                                MobleNumber=x.Mobilenumber,
                                OwnershipTypeId=x.OwnershipTypeID.Id,
                                OId = OId
                            };
                            c.DevOrganisationsPartners.InsertOnSubmit(partner);
                            c.SubmitChanges();
                        }
                        else
                        {
                            var _partner = c.DevOrganisationsPartners.SingleOrDefault(y => y.PId == x.PartnerId);
                            _partner.Email = x.Email;
                            _partner.MobleNumber = x.Mobilenumber;
                            _partner.OwnershipTypeId = x.OwnershipTypeID.Id;
                            _partner.OId = OId;
                            c.SubmitChanges();
                        }
                    });
                    scope.Complete();
                    return new Models.Common.Result
                    {
                        Status = Models.Common.Result.ResultStatus.success,
                        Message = string.Format("Partners Added Successfully!"),
                    };
                }
            }
        }


    }
}
