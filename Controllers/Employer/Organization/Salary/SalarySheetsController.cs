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
        [Route("SalarySheets/One")]
        [ApiController]
        public class OrganizationCodeController : ControllerBase
        {
            [HttpGet]
            [Route("Code")]
            public IActionResult Get()
            {
                var URId = HttpContext.Items["URId"];
                return Ok(new SalarySheets().Get(URId));
            }
        }
        //[HttpPost]
        //[Route("OrganizationDetails/Create")]
        //public IActionResult Create([FromBody] Models.Employer.Organization.Salary.SalarySheet value)
        //{
        //    var UserID = HttpContext.Items["UserID"];
        //    return Ok(new SalarySheets().Create(UserID, value));
        //}
    }
}
