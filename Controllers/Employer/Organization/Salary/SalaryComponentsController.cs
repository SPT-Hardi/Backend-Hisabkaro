using HIsabKaro.Cores.Employer.Organization.Salary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Salary
{
    [Route("Employer/Organization/Salary")]
    [ApiController]
    public class SalaryComponentsController : ControllerBase
    {
        //=======PF
        [HttpGet]
        [Route("SalaryComponents/PF/One")]
        public IActionResult PF()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().PF(URId));
        }

        [HttpPost]
        [Route("SalaryComponents/PF/Create")]
        public IActionResult PF([FromBody] Models.Employer.Organization.Salary.SalaryComponent value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().PF(URId, value));
        }

        //=======ESI
        [HttpGet]
        [Route("SalaryComponents/ESI/One")]
        public IActionResult ESI()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().ESI(URId));
        }

        [HttpPost]
        [Route("SalaryComponents/ESI/Create")]
        public IActionResult ESI([FromBody] Models.Employer.Organization.Salary.SalaryComponent value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().ESI(URId, value));
        }
        //=======Allowance
        [HttpGet]
        [Route("SalaryComponents/Allowance/One")]
        public IActionResult Allowance()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().Allowance(URId));
        }

        [HttpPost]
        [Route("SalaryComponents/Allowance/Create")]
        public IActionResult Allowance([FromBody] Models.Employer.Organization.Salary.SalaryEarningComponent value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryComponents().Allowance(URId, value));
        }
    }
}