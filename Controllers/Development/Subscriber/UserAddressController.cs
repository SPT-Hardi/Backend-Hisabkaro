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
    public class UserAddressController : ControllerBase
    {
        [HttpPost]
        [Route("UserAddress")]
        public IActionResult Post(Models.Common.Address value) 
        {
            var UID = HttpContext.Items["UserID"];
            var DeviceToken = HttpContext.Items["DeviceToken"];
            return Ok(new UserAddress().Add(value,UID.ToString(),DeviceToken.ToString()));
        }
    }
}
