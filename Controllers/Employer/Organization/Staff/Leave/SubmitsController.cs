using HisaabKaro.Cores.Employer.Organization.Staff.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Employer.Organization.Staff.Leave
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class SubmitsController : ControllerBase
    {
        [HttpPost]
        [Route("Leave/Submit")]
        public IActionResult Post(Models.Employer.Organization.Staff.Leave.Submit value) 
        {
            if (value.RId == 2 || value.RId == 3)
            {
                var UID = HttpContext.Items["UserID"];
                return Ok(new Submits().Add(value, UID.ToString()));
            }
            else 
            {
                throw new ArgumentException("User not Authorized!");
            }
        }
    }
}
