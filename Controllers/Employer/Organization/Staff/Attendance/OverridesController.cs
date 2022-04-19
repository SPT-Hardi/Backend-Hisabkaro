using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Attendance
{
    [Route("Employer/Organization/Staff/Attendance")]
    [ApiController]
    public class OverridesController : ControllerBase
    {
        [HttpPost]
        [Route("Override")]
        public IActionResult Post(Override value) 
        {
            //int URId = (int)HttpContext.Items["URId"];
            int URId = 10000024;
            return Ok(new Overrides().Add(URId,value));
        }
    }
}
