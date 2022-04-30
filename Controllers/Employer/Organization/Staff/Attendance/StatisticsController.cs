using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Attendance
{
    [Route("Employer/Organization/Staff/Attendance")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("Statistics/{date}")]
        public IActionResult Get(DateTime date) 
        {
            var URId = HttpContext.Items["URId"];
            //int URId = 10000024;
            return Ok(new Statistics().Get(URId,date));
        }
    }
}
