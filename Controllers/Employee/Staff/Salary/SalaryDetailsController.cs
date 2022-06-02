using HIsabKaro.Cores.Employee.Staff.Salary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Staff.Salary
{
    [Route("Employee/Staff/Salary")]
    [ApiController]
    public class SalaryDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("SalaryDetails/Deduction")]
        public IActionResult Deduction()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryDetails().Deduction(URId));
        }

        [HttpGet]
        [Route("SalaryDetails/Payment")]
        public IActionResult Payment()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new SalaryDetails().Payment(URId));
        }
    }
}
