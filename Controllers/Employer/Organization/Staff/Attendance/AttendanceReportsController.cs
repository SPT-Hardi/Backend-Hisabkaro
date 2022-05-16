﻿using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
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
    public class AttendanceReportsController : ControllerBase
    {
        [HttpGet]
        [Route("Report/{date}")]
        public IActionResult Get([FromRoute]DateTime date) 
        {
            var URId=HttpContext.Items["URId"];
            var Ids = HttpContext.Items["Ids"];
            return Ok(new AttendanceReports().Get(Ids,date));
        }
    }
}