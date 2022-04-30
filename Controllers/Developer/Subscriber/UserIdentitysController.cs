using HIsabKaro.Cores.Developer.Subscriber;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Developer.Subscriber
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
            return Ok(new UserIdentitys().Add( UID,value));
        }
    }
}
