using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Developer.Subscriber
{
    [Route("Development/Subscriber")]
    [ApiController]
    public class UserWorkExperiencesController : ControllerBase
    {
        [HttpPost]
        [Route("UserWorkExperience")]
        public IActionResult Post() 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok();
        }
    }
}
