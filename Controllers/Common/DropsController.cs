using HIsabKaro.Controllers.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Route("Common/Drop")]
    [ApiController]
    public class DropsController : ControllerBase
    {
        [HttpPost]
        [Route("General/Data")]
        public IActionResult General(string Con, string parentCon)
        {
            try
            {
                var exists = new Cores.Developer.Schema.Controllers().One(Con);
                if (exists is null)
                {
                    throw new HttpResponseException() { Status = 404, Value = "Controller doesn't exists!" };
                }

                if (exists.ControllerType.Id != 14)
                {
                    throw new HttpResponseException() { Status = 405, Value = "Controller not allowed to perform this operation!" };
                }

                if (exists.NeedLogin == true)
                {
                    if (Cores.Common.Context.Current.IsLoggedIn == true)
                    {
                        Cores.Common.Context.Current.SetCId(exists.CId);
                        if (!Cores.Common.Context.Current.IsAuthorised(Con, parentCon))
                        {
                            throw new HttpResponseException() { Status = 403, Value = "You are not authorised to access this feature!" };
                        }
                    }
                    else
                    {
                        var access_token = Cores.Common.Context.Current.httpContext.Request.Headers["Authorization"].ToString();
                        if (String.IsNullOrEmpty(access_token))
                        {
                            throw new HttpResponseException() { Status = 401, Value = "invalid_token" };
                        }
                        else
                        {
                            throw new HttpResponseException() { Status = 401, Value = "token_expired" };
                        }
                    }
                }

                return Ok(new Cores.Common.Drops().General(Con, value.WhereClause().SqlParameters));
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new HttpResponseException() { Status = 400, Value = ex.Message };
            }
        }
    }
}
