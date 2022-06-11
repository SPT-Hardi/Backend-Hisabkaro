using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization;
using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationDetails
    {
        public Result Create(object UserID, OrganizationDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _UserID = c.SubUsers.Where(u => u.UId == (int)UserID).SingleOrDefault();
                    if (_UserID is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    Random OrgCode = new Random();


                    var _FileId = (from x in c.CommonFiles where x.FGUID == value.Image select x).FirstOrDefault();

                    var Org = new DevOrganisation()
                    {
                        LogoFileId = _FileId == null ? null : _FileId.FileId,
                        OrganisationName = value.OrgName,
                        InudstrySectorId = value.InudstrySector.Id,
                        Latitude = value.Latitude,
                        Longitude = value.Longitude,
                        OrgCode = (OrgCode.Next(100000, 999999)).ToString(),
                        QRString = Guid.NewGuid().ToString(),
                        UId = (int)UserID,
                    };
                    c.DevOrganisations.InsertOnSubmit(Org);
                    c.SubmitChanges();

                    var _OrgRole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId == Org.OId);
                    if (_OrgRole is null)
                    {
                        var _role = new SubRole()
                        {
                            OId = Org.OId,
                            RoleName = "admin",
                            LoginTypeId = 21,//_UserID.DefaultLoginTypeId,
                        };
                        c.SubRoles.InsertOnSubmit(_role);
                        c.SubmitChanges();
                    }
                    var _ORole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId == Org.OId);
                    var _subOrg = new SubUserOrganisation()
                    {
                        UId = (int)UserID,
                        OId = Org.OId,
                        RId = _ORole.RId,
                    };
                    c.SubUserOrganisations.InsertOnSubmit(_subOrg);
                    c.SubmitChanges();

                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organisation Add Successfully"),
                        Data = new
                        {
                            Organization = new IntegerNullString
                            {
                                Id = Org.OId,
                                Text = Org.OrganisationName,
                            },
                            URId = _subOrg.URId,
                            OrgCode = Org.OrgCode,
                            QRString = Org.QRString,
                        }
                    };
                }
            }
        }
        public Result OrganiztionSectorSearch(string keyword) 
        {
            using (DBContext c = new DBContext())
            {
                var res = (from x in c.SubFixedLookups
                           where x.FixedLookupType == "IndustrySector" && x.FixedLookup.Contains(keyword)
                           orderby x.FixedLookup.IndexOf(keyword) ascending
                           select new IntegerNullString()
                           {
                               Id=x.FixedLookupId,
                               Text=x.FixedLookup.Trim(),
                           }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "organization sector search successfully!",
                    Data =res
                };
            }
        }

        public Result UpdateQR(object URId)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }

                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }
                    var _OId = c.DevOrganisations.SingleOrDefault(o => o.OId == _URId.OId);
                    if (_OId is null)
                    {
                        throw new ArgumentException("Organization Does Not Exits!");
                    }

                    _OId.QRString = Guid.NewGuid().ToString();
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organisation Add Successfully"),
                        Data = new
                        {
                            QRString = _OId.QRString,
                        }
                    };
                }
            }
        }
    }
}
