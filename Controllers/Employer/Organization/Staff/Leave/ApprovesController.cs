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
        [Route("Approve/View")]
        public IActionResult Get()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Approves().Get(URId));
        }

        [HttpPut]
        [Route("Approve/{leaveId}")]
        public IActionResult Update([FromRoute] int leaveId, [FromBody] Models.Employer.Organization.Staff.Leave.Approve value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Approves().Update(URId,leaveId, value));
        }

        [HttpPost]
        [Route("Remove/{leaveId}")]
        public IActionResult Delete([FromRoute] int leaveId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Approves().Remove(URId, leaveId));
        }
    }
}
