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
    public class ER_AppliedJobsController : ControllerBase
    {
        [Route("ViewAppliedJob/{Oid}/{Jid}")]
        [HttpGet]
        public IActionResult One(int Oid, int Jid)
        {
            return Ok(new ER_AppliedJobs().Get(Oid, Jid));
        }
    }
}
