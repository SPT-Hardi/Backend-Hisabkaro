using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.OverTime
{
    public class OverTimeDetails
    {
        public Result One(object URId)
        {
            using (DBContext c = new DBContext())
            {
                var _SId = c.SubUserOrganisations.SingleOrDefault(o => o.URId == (int)URId);
                if (_SId is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var CheckIn = (from x in c.OrgStaffsOverTimeDetails
                                   where x.URId == (int)URId
                                   select new
                                   {
                                       StaffURId = x.StaffURId,
                                       checkin = x.SubUserOrganisation_StaffURId.OrgStaffsAttendancesDailies.Where(y => Convert.ToDateTime(y.ChekIN).Date == x.OverTimeDate && y.URId == x.StaffURId).Select(y => y.ChekIN).FirstOrDefault(),
                                       CheckOut = x.SubUserOrganisation_StaffURId.OrgStaffsAttendancesDailies.Where(y => Convert.ToDateTime(y.CheckOUT).Date == x.OverTimeDate && y.URId == x.StaffURId).Select(y => y.CheckOUT).FirstOrDefault(),
                                   }).ToList();
                var _OverTime = (from x in c.OrgStaffsOverTimeDetails
                            where x.URId == (int)URId
                            select new
                            {
                                StaffURId = x.StaffURId,
                                Name=x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName,
                                Image=x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.CommonFile.FGUID,
                                OverTimeDate = x.OverTimeDate,
                                OverTimeWage = x.OverTimeWage,
                                CheckIn = x.SubUserOrganisation_StaffURId.OrgStaffsAttendancesDailies.Where(y => Convert.ToDateTime(y.ChekIN).Date == x.OverTimeDate && y.URId == x.StaffURId).Select(y => y.ChekIN).FirstOrDefault(),
                                CheckOut = x.SubUserOrganisation_StaffURId.OrgStaffsAttendancesDailies.Where(y => Convert.ToDateTime(y.CheckOUT).Date == x.OverTimeDate && y.URId == x.StaffURId).Select(y => y.CheckOUT).FirstOrDefault(),
                                OverTime = x.OverTime,
                                Amount = x.Amount,
                                URId = URId,
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data =new { _OverTime , CheckIn },

                };
            }
        }

        public Result Create(object URId, int StaffId, Models.Employer.Organization.Staff.OverTime.OverTimeDetail value)
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
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId ==(int)URId && x.SubRole.RoleName == "admin");
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
                        URId = (int)URId,
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
