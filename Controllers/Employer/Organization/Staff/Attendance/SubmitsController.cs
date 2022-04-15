using HIsabKaro.Cores.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Attendance
{
    [Route("Employer/Staff")]
    [ApiController]
    public class SubmitsController : ControllerBase
    {
        //[HttpPost]
        //[Route("Attendance/SubmitDaily")]
        //public IActionResult Post(Models.Employer.Staff.Attendance.Submit value)
        //{
        //    var UID = HttpContext.Items["UserID"];
        //    if (value.RId == 1 || value.RId == 2)
        //    {
        //        return Ok(new Submits().Add(value, UID.ToString()));
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Not Authorized");
        //    }
        //}

    }
}
