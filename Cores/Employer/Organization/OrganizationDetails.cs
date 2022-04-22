using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization;
using HIsabKaro.Services;
using HisabKaroDBContext;
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
        public Result Create(int UserID,OrganizationDetail value)
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

                    var _orgname = c.DevOrganisations.Where(o => o.OrganisationName == value.OrgName).SingleOrDefault();
                    if (_orgname is not null)
                    {
                        throw new ArgumentException($"Organisation Alredy Exits With Same Name :{value.OrgName}.");
                    }

                    var _FileId = (from x in c.CommonFiles where x.FGUID == value.Image select x).FirstOrDefault();

                    var Org = new DevOrganisation()
                    {
                        LogoFileId = _FileId ==null ? null : _FileId.FileId,
                        OrganisationName = value.OrgName,
                        InudstrySectorId = value.InudstrySector.Id,
                        Latitude = value.Latitude,
                        Longitude = value.Longitude,
                        UId = UserID,
                    };
                    c.DevOrganisations.InsertOnSubmit(Org);
                    c.SubmitChanges();

                    var _OrgRole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId== Org.OId);
                    if (_OrgRole is null)
                    {
                        var _role = new SubRole()
                        {
                            OId = Org.OId,
                            RoleName="admin",
                            LoginTypeId=_UserID.LoginTypeId,
                        };
                        c.SubRoles.InsertOnSubmit(_role);
                        c.SubmitChanges();
                    }
                    var _ORole = c.SubRoles.SingleOrDefault(x => x.RoleName.ToLower() == "admin" && x.OId == Org.OId);
                    var _subOrg = new SubUserOrganisation()
                    {
                        UId=UserID,
                        OId=Org.OId,
                        RId=_ORole.RId,
                    };
                    c.SubUserOrganisations.InsertOnSubmit(_subOrg);
                    c.SubmitChanges();
                    
                    scope.Complete();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Organisation Add Successfully"),
                        Data = new {
                            Organization = new IntegerNullString
                            {
                                Id = Org.OId,
                                Text = Org.OrganisationName
                            },
                            URId= _subOrg.URId,
                        }
                    };
                }
            }
        }
    }
}
