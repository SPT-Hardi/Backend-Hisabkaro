using HIsabKaro.Cores.Employer.Organization.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Job
{
    [Route("Employer/Organization/Job")]
    [ApiController]
    public class ER_ShortListedsController : ControllerBase
    {
        [HttpPost]
        [Route("Add/Applicant_To_ShortList/{UId}/{JId}")]
        public IActionResult Post([FromRoute]int UId,[FromRoute]int JId) 
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_ShortListeds().Add(URId,UId,JId));
        }

        [HttpGet]
        [Route("Get/ShortListed_Applicants/{JId}")]
        public IActionResult Get([FromRoute] int JId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_ShortListeds().Get(URId,JId));
        }
    }
}
