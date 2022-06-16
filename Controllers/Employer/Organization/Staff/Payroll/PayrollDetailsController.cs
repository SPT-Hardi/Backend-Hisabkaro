using HIsabKaro.Cores.Employer.Organization.Staff.Payroll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Payroll
{
    [Route("Employer/Organization/Staff/Payroll")]
    [ApiController]
    public class PayrollDetailsController : ControllerBase
    {
        //=======PF
        [HttpGet]
        [Route("PayrollDetails/PF/One")]
        public IActionResult PFOne()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new PayrollDetails().PFOne(URId));
        }

        [HttpPost]
        [Route("PayrollDetails/PF/Create")]
        public IActionResult PFCreate([FromBody] Models.Employer.Organization.Staff.Payroll.PayrollDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new PayrollDetails().PFCreate(URId, value));
        }

        //=======ESI
        [HttpGet]
        [Route("PayrollDetails/ESI/One")]
        public IActionResult ESIOne()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new PayrollDetails().ESIOne(URId));
        }

        [HttpPost]
        [Route("PayrollDetails/ESI/Create")]
        public IActionResult ESICreate([FromBody] Models.Employer.Organization.Staff.Payroll.PayrollDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new PayrollDetails().ESICreate(URId, value));
        }
    }
}
