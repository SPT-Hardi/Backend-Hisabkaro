using HIsabKaro.Cores.Employer.Organization.Staff.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Leave
{
    [Route("Employer/Organization/Staff/Leave")]
    [ApiController]
    public class SubmitsController : ControllerBase
    {
        [HttpPost]
        [Route("Submit/{StaffId}")]
        public IActionResult Post([FromRoute] int StaffId, [FromBody] Models.Employer.Organization.Staff.Leave.Submit value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new Submits().Create(URId,StaffId,value));
        }
    }
}
