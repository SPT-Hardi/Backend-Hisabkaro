using HIsabKaro.Cores.Employer.Organization.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class StaffDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("StaffDetails/One")]
        public IActionResult One([FromQuery] int OId)
        {
            return Ok(new StaffDetails().One(OId));
        }

        [HttpPost]
        [Route("StaffDetails/Create")]

        public IActionResult Create([FromBody] Models.Employer.Organization.Staff.StaffDetail value)
        {
            //int UserID = (int)HttpContext.Items["UserID"];
            int UserID = 50000001;
            int RId = 10000001;
            return Ok(new StaffDetails().Create(value));
        }
    }
}
