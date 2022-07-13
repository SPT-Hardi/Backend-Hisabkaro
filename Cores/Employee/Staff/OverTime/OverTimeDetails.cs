using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using HIsabKaro.Models.Employee.OverTime;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;

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

                    var OverTimess = (from x in c.OrgStaffsAttendancesDailies
                                      where x.URId == _URId.URId && x.ChekIN.Month == ISDT.Month && x.ChekIN.Year == ISDT.Year && (x.IsOvertimeFullDay == true || x.IsOvertimeHalfDay == true)
                                      select x).ToList();

                    OverTimess.ForEach(x =>
                        overTime.Add(new OverTimeDetail(){
                            Date=x.ChekIN.Date,
                            CheckIn = x.ChekIN,
                            CheckOut = x.CheckOUT,
                            Hours = x.CheckOUT == null ? null : (x.CheckOUT.Value.TimeOfDay - x.ChekIN.TimeOfDay)
                        })
                    ); 
                   
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format("Over TIme Details"),
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
