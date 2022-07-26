using HIsabKaro.Controllers.Filters;
using HIsabKaro.Controllers.Filters.Custom;
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
        /*[HttpPost]
        [Route("Override")]
        [Authenticate("Employer-Organization-Staff-Attendance-Overrides")]
        public IActionResult Post(Override value) 
        {
            var URId = HttpContext.Items["URId"];
            var Ids = HttpContext.Items["Ids"];
            //int URId = 10000024;
            new HaveAuthority().AccessStaff(Ids);
            return Ok(new Overrides().Add(URId,value));
        }*/
    }
}
