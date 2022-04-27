using HIsabKaro.Cores.Common;
using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
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
    public class SubmitsController : ControllerBase
    {
       [HttpPost]
        [Route("SubmitDaily")]
        public IActionResult Post([FromBody]SubmitDaily value)
        {
            int URId = (int)HttpContext.Items["URId"];
            //int URId = 10000024;
            return Ok(new Submits().Add(URId,value));
        }

        [HttpGet]
        [Route("SubmitDaily")]
        public IActionResult Get()
        {
            int URId = (int)HttpContext.Items["URId"];
            //int URId = 10000024;
            return Ok(new Submits().Get(URId));
        }

        [HttpPost]
        [Route("SubmitDailyQR")]
        public IActionResult PostQR([FromBody] SubmitDailyThroughQR value)
        {
            int URId = (int)HttpContext.Items["URId"];
            //int URId = 10000024;
            return Ok(new Submits().AddFromQr(URId,value));
        }

    }
}
