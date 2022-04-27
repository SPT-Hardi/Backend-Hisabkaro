using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Advance
{
    public class AdvanceDetails
    {
        public Result One(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var _SId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == URId);
                if (_SId is null)
                {
                    throw new ArgumentException("Satff Does Not Exits!");
                }

                var _Bonus = (from x in c.OrgStaffsAdvanceDetails
                                 where x.URId == URId
                                 select new
                                 {
                                     AdvanceId = x.AdvanceId,
                                     StaffURId = x.StaffURId,
                                     FullName = x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName,
                                     Image = x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.CommonFile.FilePath,
                                     Date=x.Date,
                                     Description = x.Description,
                                     Amount = x.Amount,
                                     URId = URId,
                                 }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Bonus,

                };
            }
        }

        public Result Create(int URId,int StaffId, Models.Employer.Organization.Staff.Advance.AdvanceDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId && x.SubRole.RoleName=="admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == StaffId && x.SubUserOrganisation.OId==_URId.OId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    var _Advance = new OrgStaffsAdvanceDetail()
                    {
                        StaffURId=StaffId,
                        Date = value.Date,
                        Amount=value.Amount,
                        Description=value.Description,
                        URId= URId,
                    };
                    c.OrgStaffsAdvanceDetails.InsertOnSubmit(_Advance);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Advance Give Successfully!"),
                    };
                }
            }
        }
    }
}
