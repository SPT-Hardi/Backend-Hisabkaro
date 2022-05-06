using HIsabKaro.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Route("Common/User/LoginType")]
    [ApiController]
    public class LoginTypesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() 
        {
            var Ids = HttpContext.Items["Ids"];
            HIsabKaro.Models.Common.Ids ids = (Models.Common.Ids)Ids;
            return Ok(new IntegerNullString() { Id=ids.LId,Text=null,});
        }
    }
}
