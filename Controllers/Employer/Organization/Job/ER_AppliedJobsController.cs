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
        [Route("ViewAppliedJob/{Jid}")]
        [HttpGet]
        public IActionResult One(int Jid)
        {
            int Uid = 50000001;
            int Rid = 1000004;
            return Ok(new ER_AppliedJobs().Get(Jid, Uid, Rid));
        }
    }
}
