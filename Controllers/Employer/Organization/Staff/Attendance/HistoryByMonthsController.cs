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
    public class HistoryByMonthsController : ControllerBase
    {
        [HttpGet]
        [Route("HistoyOfMonth/{date}")]
        public IActionResult Get([FromRoute]DateTime date) 
        {
            //int URId = (int)HttpContext.Items["URId"];
            int URId = 10000024;
            return Ok(new HistoryByMonths().Get(URId,date));
        }
    }
}