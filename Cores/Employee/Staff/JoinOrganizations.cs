using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff
{
    public class JoinOrganizations
    {
        /* public Result Add(object UId, IConfiguration configuration, ITokenServices tokenServices)
         {
             using (DBContext c = new DBContext())
             {
                 Cores.Common.Claims claims = new Claims(configuration, tokenServices);
                 using (TransactionScope scope = new TransactionScope())
                 {
                     scope.Complete();

                     return new Result()
                     {
                         Status = Result.ResultStatus.success,
                         Message = string.Format("Staff Add Successfully!"),
                         Data = new
                         {
                             UserName = username,
                             JWT = res.JWT,
                             RToken = res.RToken,
                         }
                     };
                 }
             }
         }*/
    }
}
