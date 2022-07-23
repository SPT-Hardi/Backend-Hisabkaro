using HIsabKaro.Controllers.Filters;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamIN.ReckonIN.API.Helpers.Kendo;

namespace HIsabKaro.Controllers.Common
{
    [Route("Common/Drop")]
    [ApiController]
    public class DropsController : ControllerBase
    {
        [HttpPost]
        [Route("General/Data")]
        public IActionResult General(string Con, string parentCon, [DataSourceRequest] DataSourceRequest value)
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
                    if (Cores.Common.Contact.Current.IsLoggedIn == true)
                    {
                        Cores.Common.Contact.Current.SetCId(exists.CId);
                        if (!Cores.Common.Contact.Current.IsAuthorised(Con, parentCon))
                        {
                            throw new HttpResponseException() { Status = 403, Value = "You are not authorised to access this feature!" };
                        }
                    }
                    else
                    {
                        var access_token = Cores.Common.Contact.Current.httpContext.Request.Headers["Authorization"].ToString();
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
               // var Id = HttpContext.Items["Ids"];
                return Ok(new Cores.Common.Drops().General(Con, value.WhereClause().SqlParameters));
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new HttpResponseException() { Status = 400, Value = ex.Message };
            }
        }
        [HttpGet]
        [Route("Profile/{Id}")]
        public IActionResult Get(int Id) 
        {
            var UId = HttpContext.Items["UId"];
            var URId = HttpContext.Items["URId"];
            return Ok(new Cores.Common.Drops().ProfileDrop(UId,Id,URId));
        }
    }
}
