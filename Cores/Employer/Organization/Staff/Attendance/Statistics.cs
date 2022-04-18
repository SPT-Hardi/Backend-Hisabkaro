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
        public Result Get(int URId) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var attendancelist = c.OrgStaffsAttendancesDailies.Where(x => x.URId ==URId).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Staff attendance-statistics get successfully!",
                        Data = "",
                    };
                }
            }
        }
    }
}
