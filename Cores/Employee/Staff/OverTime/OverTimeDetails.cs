using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using HIsabKaro.Models.Employee.OverTime;

namespace HIsabKaro.Cores.Employee.Staff.OverTime
{
    public class OverTimeDetails
    {
        public Result One(object URId, DateTime Date)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var ISDT = new Common.ISDT().GetISDT(Date);

                    var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName.ToLower() == "staff");
                    if (_URId is null)
                    {
                        throw new ArgumentException("Unathorized!");
                    }

                    List<OverTimeDetail> overTime = new List<OverTimeDetail>();

                    var OverTime = (from x in c.OrgStaffsOverTimeDetails
                                    where x.StaffURId == _URId.URId && x.OverTimeDate.Month == ISDT.Month && x.OverTimeDate.Year == ISDT.Year
                                    select x).ToList();

                    OverTime.ForEach((x) => {
                        var Attendance = (from y in c.OrgStaffsAttendancesDailies
                                          where y.ChekIN.Value.Date == x.OverTimeDate && y.URId == _URId.URId
                                          select new { ChekIN = y.ChekIN, CheckOUT = y.CheckOUT }).FirstOrDefault();

                        overTime.Add(new OverTimeDetail()
                        {
                            Date = x.OverTimeDate,
                            CheckIn = Attendance == null ? null : (Attendance.ChekIN == null ? null : Attendance.ChekIN.Value),
                            CheckOut = Attendance == null ? null : (Attendance.CheckOUT == null ? null : Attendance.CheckOUT.Value),
                            Hours = Attendance == null ? null : (Attendance.CheckOUT == null ? null : (Attendance.CheckOUT.Value.TimeOfDay - Attendance.ChekIN.Value.TimeOfDay).ToString()),
                        });
                    });

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Data = new
                        {
                            OverTime = overTime
                        },
                    };
                }
            }
        }
    }
}
