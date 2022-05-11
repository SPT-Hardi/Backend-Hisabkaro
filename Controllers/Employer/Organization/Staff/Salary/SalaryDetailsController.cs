using HIsabKaro.Cores.Employer.Organization.Staff.Salary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Salary
{
    [Route("Employer/Organization/Staff/Salary")]
    [ApiController]
    public class SalaryDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("SalaryDetails/One")]
        public IActionResult One()
        [Route("SalaryDetails/Pendding")]
        public IActionResult One([FromQuery] int OId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryDetails().SalarySlip(URId));
            return Ok(new SalaryDetails().Pendding(OId));
        }

        [HttpPost]
        [Route("SalaryDetails/Create/{StaffId}")]
        public IActionResult Create([FromRoute] int StaffId, [FromBody] Models.Employer.Organization.Staff.Salary.SalaryDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryDetails().Create(URId, StaffId, value));
        }
    }
}
