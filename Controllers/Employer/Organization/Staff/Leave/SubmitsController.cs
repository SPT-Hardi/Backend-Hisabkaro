using HIsabKaro.Cores.Employer.Organization.Staff.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Leave
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class SubmitsController : ControllerBase
    {
        [HttpPost]
        [Route("Leave/Submit")]
        public IActionResult Post(Models.Employer.Organization.Staff.Leave.Submit value)
        {
            // var UID = HttpContext.Items["UserID"];Uid, Rid,
            int Uid = 50000003;
            int Rid = 1000002;
            return Ok(new Submits().Create(value,Uid));
        }
    }
}
