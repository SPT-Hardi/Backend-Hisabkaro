using HIsabKaro.Cores.Common.Contact;
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
                         select new
                         {
                             Organization = new IntegerNullString { Id = x.OId, Text = x.OrganisationName },
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
                                                                select new IntegerNullString { Id = l.LookupId, Text = l.Lookup }).SingleOrDefault()
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
        public Result Create(int UserId,int OId,Models.Employer.Organization.OrganizationProfile value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope=new TransactionScope())
                {
                    var _User = c.SubUsers.Where(u => u.UId == UserId).SingleOrDefault();
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == OId);
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
                    var _GSTFileId = (from x in c.CommonFiles where x.FGUID == value.GST select x).FirstOrDefault();
                    var _PANFileId = (from x in c.CommonFiles where x.FGUID == value.PanCard select x).FirstOrDefault();

                    _OId.GSTFileId = _GSTFileId==null ? null : _GSTFileId.FileId;
                    _OId.GSTIN = value.GSTNumber;
                    _OId.ContactAddressId = _AId.Data;
                    _OId.PAN = value.PanCardNumber;
                    _OId.PANFileId = _PANFileId.FileId;
                    _OId.Email = value.Email;
                    _OId.MobileNumber = value.MobileNumber;
                    
                    c.SubmitChanges();

                    c.DevOrganisationsPartners.InsertAllOnSubmit(value.Partners.Select(x => new DevOrganisationsPartner()
                    {
                        OId = OId,
                        OwnershipTypeId=x.OwnershipTypeID.Id,
                        Email=x.Email,
                        MobleNumber=x.Mobilenumber,
                    }).ToList());
                    c.SubmitChanges();

                    var _OrgRole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId == OId);
                    var _URID = c.SubUserOrganisations.SingleOrDefault(x => x.UId == UserId && x.OId == OId && x.RId == _OrgRole.RId);
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
    }
}
