using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationDetails
    {
        public Result Create(int UserID,OrganizationDetail value)
        {
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
            {
                var _UserID = context.SubUsers.Where(u => u.UId == UserID).SingleOrDefault();
                if (_UserID is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var _orgname = context.DevOrganisations.Where(o => o.OrganisationName == value.OrgName).SingleOrDefault();
                if (_orgname is not null)
                {
                    throw new ArgumentException($"Organization Are Alredy Exits With Same Name :{value.OrgName}.");
                }

                var _FileId = context.CommonFiles.SingleOrDefault(x=>x.FGUID==value.Image);
                var Org = new DevOrganisation()
                {
                    LogoFileId= _FileId.FileId,
                    OrganisationName = value.OrgName,
                    InudstrySectorId= value.InudstrySector.ID,
                    Latitude = value.Latitude,
                    Longitude = value.Longitude,
                    UId=UserID,
                };
                context.DevOrganisations.InsertOnSubmit(Org);
                context.SubmitChanges();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format($"Organization Add Successfully"),
                    Data=new { Id=Org.OId }
                };
            }
        }
    }
}
