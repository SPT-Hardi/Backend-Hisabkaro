using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Attendance
{
    public class Statistics
    {
        public Result Get(int URId,DateTime date) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    int presentcount = 0;
                    var totalemp = (from obj in c.SubUserOrganisations
                                    join obj1 in c.SubRoles
                                    on obj.OId equals obj1.OId
                                    where obj.URId == URId && obj1.RId == 1000001
                                    select obj).ToList();
                    foreach (var item in totalemp) 
                    {
                        var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.URId == item.URId && x.ChekIN.Value.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy")).SingleOrDefault();
                        if (checkpresent != null) 
                        {
                            presentcount += presentcount;
                            var lateafter = c.DevOrganisationsShiftTimes.Where(x => x.OId == item.OId).SingleOrDefault();
                            
                        }
                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance-statistics get successfully!",
                        Data = new 
                        {
                            TotalEmployee=totalemp.Count(),
                            Present=presentcount,
                            Absent=totalemp.Count()-presentcount,
                          
                        },
                    };
                }
            }
        }
    }
}
