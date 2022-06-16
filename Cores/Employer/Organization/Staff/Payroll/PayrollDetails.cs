using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Payroll
{
    public class PayrollDetails
    {
        public Result PFCreate(object URId, List<Models.Employer.Organization.Staff.Payroll.PayrollDetail> value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var p = (from x in c.PayrollFixedLookups
                             select x).ToList();
                }
                return new Result() { };
            }
        } 
    }
}
