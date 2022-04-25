using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.OverTime
{
    public class OverTimeDetails
    {
        public Result One(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var _SId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == URId);
                if (_SId is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var _OverTime = (from x in c.OrgStaffsOverTimeDetails
                            where x.URId == URId
                            select new
                            {
                                StaffURId = x.StaffURId,
                                Name=x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName,
                                Image=x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.CommonFile.FilePath,
                                OverTimeDate = x.OverTimeDate,
                                OverTimeWage = x.OverTimeWage,
                                OverTime = x.OverTime,
                                Amount = x.Amount,
                                URId = URId,
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _OverTime,

                };
            }
        }

        public Result Create(int URId, int StaffId, Models.Employer.Organization.Staff.OverTime.OverTimeDetail value)
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
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId && x.SubRole.RoleName == "admin");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == StaffId && x.SubUserOrganisation.OId == _URId.OId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }
                    var time= Convert.ToDecimal(TimeSpan.Parse($"{value.Time}").Hours)+"."+Convert.ToDecimal(TimeSpan.Parse($"{value.Time}").Minutes);
                    var t = Convert.ToDecimal((TimeSpan.Parse($"{value.Time}").Hours) + "." + (TimeSpan.Parse($"{value.Time}").Minutes)) * value.OverTimeWage;

                    var _OverTime = new OrgStaffsOverTimeDetail()
                    {
                        StaffURId = StaffId,
                        OverTimeDate = value.Date,
                        OverTimeWage = value.OverTimeWage,
                        OverTime = (TimeSpan)value.Time,
                        Amount = Convert.ToDecimal((TimeSpan.Parse($"{value.Time}").Hours) + "." + (TimeSpan.Parse($"{value.Time}").Minutes)) * value.OverTimeWage,
                        URId = URId,
                    };
                    c.OrgStaffsOverTimeDetails.InsertOnSubmit(_OverTime);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Over Time Give Successfully!"),
                    };
                }
            }
        }

    }
}
