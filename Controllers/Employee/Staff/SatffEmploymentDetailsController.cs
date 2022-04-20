using HIsabKaro.Cores.Employee.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Staff
{
    [Route("Employee/Staff")]
    [ApiController]
    public class SatffEmploymentDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("SatffEmploymentDetails/One")]
        public IActionResult One()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new SatffEmploymentDetails().One(URId));
        }

        [HttpPost]
        [Route("SatffEmploymentDetails/Create")]
        public IActionResult Create([FromBody] Models.Employee.Staff.SatffEmploymentDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new SatffEmploymentDetails().Create(URId, value));
        }
    }
}
