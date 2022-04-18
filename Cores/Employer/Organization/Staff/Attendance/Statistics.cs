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
                    int latecount = 0;
                    var findorg = c.SubUserOrganisations.Where(x => x.URId == URId).SingleOrDefault();
                    var findstaffroleid = c.SubRoles.Where(x => x.RoleName.ToLower() == "staff" && x.OId == findorg.OId).SingleOrDefault();
                    var totalemp = (from obj in c.SubUserOrganisations
                                    where obj.OId== findorg.OId && obj.RId == findstaffroleid.RId
                                    select obj).ToList();
                    foreach (var item in totalemp) 
                    {
                        var todaydate = date.ToString("dd/MM/yyyy");
                        
                        var checkpresent = c.OrgStaffsAttendancesDailies.Where(x => x.URId == item.URId).SingleOrDefault();
                        if (checkpresent.ChekIN.Value.ToString("dd/MM/yyyy")==todaydate) 
                        {
                            presentcount += 1;

                            var lateafter = (from obj in c.DevOrganisationsStaffs
                                             join obj1 in c.DevOrganisationsShiftTimes
                                             on obj.ShiftTimeId equals obj1.ShiftTimeId
                                             where obj.OId==item.OId
                                             select obj1).SingleOrDefault();
                            if (checkpresent.ChekIN.Value.TimeOfDay > lateafter.MarkLate) 
                            {
                                latecount += 1;
                            }
                            
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
                            Late=latecount,
                          
                        },
                    };
                }
            }
        }
    }
}
