using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class AttendanceReports
    {
        public Result Get(object Ids,DateTime date) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                var ids = (Models.Common.Ids)Ids;
                List<AttendaneReport> attendaneReports = new List<AttendaneReport>();
                using (DBContext c = new DBContext())
                {
                    var totalemp = (from obj in c.DevOrganisationsStaffs
                                    where obj.OId == ids.OId && obj.SubUserOrganisation.SubRole.RoleName.ToLower() == "staff" && obj.CreateDate.Month <= date.Month && obj.CreateDate.Year<=date.Year
                                    select obj).ToList();
                    foreach (var item in totalemp) 
                    {
                        var res = new HistoryByMonths().Get(item.URId, 0, date);
                        attendaneReports.Add(new AttendaneReport()
                        {
                            URId=item.URId,
                            Name=item.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                            Present=res.Data.Present,
                            Absent=res.Data.Absent,
                            Late=res.Data.Late,
                            Overtime=res.Data.OverTime,
                            WeekOff=res.Data.WeeklyOff,
                            TotalWorkingHours= res.Data.TotalWorkingHourPerMonth,
                        });
                    }
                }
                scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staffs-Attendance report get successfully!",
                        Data = attendaneReports,
                    };
            }
        }
    }
}
