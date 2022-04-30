using HIsabKaro.Cores.Employer.Organization.Staff.Advance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Advance
{
    [Route("Employer/Organization/Staff/Advance")]
    [ApiController]
    public class AdvanceDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("AdvanceDetails/One")]
        public IActionResult One()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new AdvanceDetails().One(URId));
        }

        [HttpPost]
        [Route("AdvanceDetails/Create/{StaffId}")]
        public IActionResult Create([FromRoute]int StaffId, [FromBody] Models.Employer.Organization.Staff.Advance.AdvanceDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new AdvanceDetails().Create(URId, StaffId, value));
        }
    }
}
