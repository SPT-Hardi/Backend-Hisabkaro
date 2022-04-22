using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Bonus
{
    public class BounsDetails
    {
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
                                URId = URId,
                                DOB = x.DOB,
                                Gender = x.Gender,
                                Address = (from y in c.CommonContactAddresses
                                           where y.ContactAddressId == x.SubUserOrganisation.SubUser.SubUsersDetail.AddressID
                                           select new
                                           {
                                               AddressLine1 = y.AddressLine1,
                                               AddressLine2 = y.AddressLine2,
                                               City = y.City,
                                               State = y.State,
                                               PinCode = y.PinCode,
                                               LandMark = y.Landmark
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

        public Result Create(int URId,int StaffId, Models.Employer.Organization.Staff.Bonus.BounsDetail value)
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

                    var _Bouns = new OrgStaffsBonusDetail()
                    {
                        URId=StaffId,
                        Date = value.Date,
                        Amount=value.Amount,
                        Description=value.Description,
                    };
                    c.OrgStaffsBonusDetails.InsertOnSubmit(_Bouns);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Bouns Give Successfully!"),
                    };
                }
            }
        }
    }
}
