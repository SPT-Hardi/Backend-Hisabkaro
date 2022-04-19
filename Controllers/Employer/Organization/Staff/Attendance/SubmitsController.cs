using HIsabKaro.Cores.Common;
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
    public class SubmitsController : ControllerBase
    {
        [HttpPost]
        [Route("SubmitDaily")]

        public IActionResult Post([FromBody] Models.Employer.Organization.Staff.Attendance.Submit value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new Submits().Add(URId, value));
        }

    }
}
