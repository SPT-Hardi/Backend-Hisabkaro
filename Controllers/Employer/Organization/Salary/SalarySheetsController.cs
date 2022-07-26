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
    public class SalarySheetsController : ControllerBase
    {
       /* [HttpGet]
        [Route("SalarySheets/One")]
        public IActionResult One()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalarySheets().SalarySlip(URId));
        }*/
      /*  [Route("SalarySheets/Pending")]
        [HttpGet]
        public IActionResult Get()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalarySheets().Pending(URId));
        }*/

        [HttpPost]
        [Route("SalarySheets/Create/{StaffURId}")]
        public IActionResult Create([FromRoute] int StaffURId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalarySheets().Create(URId,StaffURId));
        }
    }
}
