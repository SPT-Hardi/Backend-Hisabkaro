using HIsabKaro.Cores.Employer.Organization.Staff.Bonus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Bonus
{
    [Route("Employer/Organization/Staff/Bonus")]
    [ApiController]
    public class BounsDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("BounsDetails/One")]
        public IActionResult One()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new BounsDetails().One(URId));
        }

        [HttpPost]
        [Route("BounsDetails/Create/{StaffId}")]
        public IActionResult Create([FromRoute]int StaffId, [FromBody] Models.Employer.Organization.Staff.Bonus.BounsDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new BounsDetails().Create(URId, StaffId, value));
        }
    }
}
