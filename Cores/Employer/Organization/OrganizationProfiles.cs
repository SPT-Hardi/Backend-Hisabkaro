using CsvHelper;
using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization;
using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using static HIsabKaro.Cores.Developer.Subscriber.Users;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationProfiles
    {
        public enum DocumentName
        {
            GST = 169,
            PAN = 168
        }
        public Result Create(object UId, object DeviceToken, OrganizationProfile value, ITokenServices tokenServices, IConfiguration configuration)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null || DeviceToken == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                    if (user == null)
                    {
                        throw new ArgumentException("user not found!");
                    }
                    Random orgCode = new Random();
                    Random brcCode = new Random();
                    //create new Org
                    var org = new DevOrganisation()
                    {
                        ContactAddressId = value.AddressId,
                        Email = user.SubUsersDetail.Email,
                        IsBranch = false,
                        LogoFileId = (from x in c.CommonFiles where x.FGUID == value.LogoFGUId select x).FirstOrDefault()?.FileId,
                        MobileNumber = user.MobileNumber,
                        OrganisationName = value.OrganizationName,
                        OrgCode = (orgCode.Next(100000, 999999)).ToString(),
                        QRString = new Guid().ToString(),
                        ParentOrgId = null,
                        SectorId = value.Sector.Id,
                        UId = user.UId,
                    };
                    c.DevOrganisations.InsertOnSubmit(org);
                    c.SubmitChanges();

                    //make role entry in role table
                    //1.Employer
                    var orgAdmin = new SubRole()
                    {
                        IsLocked = false,
                        LoginTypeId = (int)LoginType.Employer,
                        OId = org.OId,
                        RoleName = "Admin",
                    };
                    c.SubRoles.InsertOnSubmit(orgAdmin);
                    c.SubmitChanges();
                    //2.Partner
                    var orgPartner = new SubRole()
                    {
                        IsLocked = false,
                        LoginTypeId = (int)LoginType.Partner,
                        OId = org.OId,
                        RoleName = "Partner",
                    };
                    c.SubRoles.InsertOnSubmit(orgAdmin);
                    c.SubmitChanges();
                    //3.Staff
                    c.SubRoles.InsertOnSubmit(new SubRole()
                    {
                        IsLocked = false,
                        LoginTypeId = (int)LoginType.Staff,
                        OId = org.OId,
                        RoleName = "Staff",
                    });
                    c.SubmitChanges();
                    //4.Manager
                    c.SubRoles.InsertOnSubmit(new SubRole()
                    {
                        IsLocked = false,
                        LoginTypeId = (int)LoginType.Manager,
                        OId = org.OId,
                        RoleName = "Manager",
                    });
                    c.SubmitChanges();

                    //create user_Organization for role Admin
                    var orgAdmin_URId = new SubUserOrganisation()
                    {
                        OId = org.OId,
                        RId = orgAdmin.RId,
                        UId = user.UId
                    };
                    c.SubUserOrganisations.InsertOnSubmit(orgAdmin_URId);
                    c.SubmitChanges();

                    //generate token for admin
                    var tkn = new Cores.Common.Claims(configuration, tokenServices).Add(user.UId, DeviceToken.ToString(), orgAdmin_URId.URId);

                    //create weekoff for Organization
                    c.DevOrganizationWeekOffs.InsertAllOnSubmit(value.WeekOff.Where(z => z.Id != 0 || z.Text != null).Select(z => new DevOrganizationWeekOff()
                    {
                        Created = DateTime.Now,
                        LastUpdated = DateTime.Now,
                        OId = org.OId,
                        URId = orgAdmin_URId.URId,
                        WeekOffDay = z.Text,
                        WeekOffDayId = z.Id,
                    }));
                    c.SubmitChanges();

                    //create shiftTime for organization
                    c.DevOrganisationsShiftTimes.InsertAllOnSubmit(value.ShiftTime.Where(z => z.MarkLate != null || z.StartTime != null || z.EndTime != null).Select(z => new DevOrganisationsShiftTime()
                    {
                        Created = DateTime.Now,
                        EndTime = z.EndTime,
                        MarkLate = z.MarkLate,
                        OId = org.OId,
                        LastUpdated = DateTime.Now,
                        StartTime = z.StartTime,
                        URId = orgAdmin_URId.URId,
                    }));
                    c.SubmitChanges();

                    //add new branches
                    c.DevOrganisations.InsertAllOnSubmit(value.Branches.Where(z => z.BranchName != null).Select(z => new DevOrganisation()
                    {
                        ContactAddressId = z.AddressId,
                        Email = user.SubUsersDetail.Email,
                        IsBranch = true,
                        LogoFileId = null,
                        MobileNumber = user.MobileNumber,
                        OrganisationName = z.BranchName,
                        OrgCode = (brcCode.Next(100000, 999999)).ToString(),
                        QRString = new Guid().ToString(),
                        ParentOrgId = org.OId,
                        SectorId = null,
                        UId = user.UId,
                    }));
                    c.SubmitChanges();

                    //add partners in Organization
                    c.DevOrganisationsPartners.InsertAllOnSubmit(value.Partners.Where(z => z.PartnerName != null).Select(z => new DevOrganisationsPartner()
                    {
                        MobleNumber = z.Mobilenumber,
                        OId = org.OId,
                        PartnerName = z.PartnerName,
                        PartnerURId = null, //can be change for permissions 
                    }));
                    c.SubmitChanges();

                    //add OrganizationInformation ---GST
                    c.DevOrganizationInfoDocs.InsertOnSubmit(new DevOrganizationInfoDoc()
                    {
                        DocumentFileId = (from x in c.CommonFiles where x.FGUID == value.OrgInformation.GSTFGUId select x).FirstOrDefault()?.FileId,
                        DocumentName = "GST",
                        DocumentNameId = (int)DocumentName.GST,
                        DocumentNumber = value.OrgInformation.GSTNumber,
                        OId = org.OId,
                        URId = orgAdmin_URId.URId,
                    });
                    c.SubmitChanges();
                    //add OrganizationInformation ---PAN
                    c.DevOrganizationInfoDocs.InsertOnSubmit(new DevOrganizationInfoDoc()
                    {
                        DocumentFileId = (from x in c.CommonFiles where x.FGUID == value.OrgInformation.PANFGUId select x).FirstOrDefault()?.FileId,
                        DocumentName = "PAN",
                        DocumentNameId = (int)DocumentName.PAN,
                        DocumentNumber = value.OrgInformation.PANNumber,
                        OId = org.OId,
                        URId = orgAdmin_URId.URId,
                    });
                    c.SubmitChanges();

                    var res = new
                    {
                        OrganizationId = org.OId,
                        OrganizationName = org.OrganisationName,
                        URId = orgAdmin_URId.URId,
                        JWT = tkn.JWT,
                        RToken = tkn.RToken,
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Organization created successfully!",
                        Data = res
                    };
                }
            }
        }
    }
}
