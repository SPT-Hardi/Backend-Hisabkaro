using HIsabKaro.Cores.Employer.Organization.Staff.OverTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.OverTime
{
    [Route("Employer/Organization/Staff/OverTime")]
    [ApiController]
    public class OverTimeDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("OverTimeDetails/One")]
        public IActionResult One()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new OverTimeDetails().One(URId));
        }

        [HttpPost]
        [Route("OverTimeDetails/Create/{StaffId}")]
        public IActionResult Create([FromRoute] int StaffId, [FromBody] Models.Employer.Organization.Staff.OverTime.OverTimeDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new OverTimeDetails().Create(URId, StaffId, value));
        }
    }
}
