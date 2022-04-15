using HIsabKaro.Cores.Developer.Subscriber;
using HIsabKaro.Models.Developer.Subscriber;
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
    public class UserCompleteProfilesController : ControllerBase
    {
        [HttpPost]
        [Route("User/CompleteProfile")]
        public IActionResult Post(UserCompleteProfile value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new UserCompleteProfiles().Add(UID.ToString(),value));
        }
    }
}
