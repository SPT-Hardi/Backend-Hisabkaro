using HIsabKaro.Cores.Employer.Organization.Staff.Leave;
using HIsabKaro.Models.Employer.Organization.Staff.Leave;
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
    public class ApprovesController : ControllerBase
    {
        [HttpGet]
        [Route("ViewRequest")]
        public IActionResult Get()
        {
            int Uid = 50000003;
            int Rid = 1000002;
            return Ok(new Approves().Get(Uid, Rid));
        }

        [HttpPut]
        [Route("Approve/{leaveId}")]
        public IActionResult Update([FromRoute] int leaveId, [FromBody] Models.Employer.Organization.Staff.Leave.Approve value)
        {
            int Uid = 50000001;
            int Rid = 1000001;
            return Ok(new Approves().Update(Uid, Rid, leaveId, value));
        }
    }
}
