using HisaabKaro.Cores.Development.Subscriber;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Development.Subscriber
{
    [Route("Developer/Subscriber")]
    [ApiController]
    public class UserIdentitysController : ControllerBase
    {
        [HttpPost]
        [Route("UserIdentity")]
        public IActionResult Add([FromBody]Models.Developer.Subscriber.UserIdentity value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new UserIdentitys().Add(value,UID.ToString()));
        }
    }
}
