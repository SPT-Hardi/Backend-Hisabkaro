using HIsabKaro.Cores.Employee.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Profile
{
    [Route("Employee/Profile")]
    [ApiController]
    public class ViewProfilesController : ControllerBase
    {
        [Route("ViewProfile")]
        [HttpGet]
        public IActionResult Get()
        {
            var UserId = HttpContext.Items["UserID"];
            return Ok(new ViewProfiles().Get(UserId));
        }
    }
}
