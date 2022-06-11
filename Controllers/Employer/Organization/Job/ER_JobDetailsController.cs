using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.File;
using HIsabKaro.Cores.Employer.Organization.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Job
{
    [Route("Employer/Organization")]
    [ApiController]
    public class ER_JobDetailsController : ControllerBase
    {
        [Route("Job")]
        [HttpPost]
        public IActionResult Create(Models.Employer.Organization.Job.ER_JobDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().Create(URId,value));
        }

        [Route("Job/{Jid}")]
        [HttpPut]
        public IActionResult Update(int Jid,Models.Employer.Organization.Job.ER_JobDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().Update(URId, Jid,value));
        }

        [Route("OrgJobList")]
        [HttpGet]
        public IActionResult Get()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().One(URId));
        }

        [Route("JobDetail/{Jid}")]
        [HttpGet]
        public IActionResult GetById([FromRoute]int Jid)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().GetJob(URId,Jid));
        }

        [Route("RemoveJob/{Jid}")]
        [HttpPost]
        public IActionResult Remove([FromRoute]int Jid)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().RemovePost(URId, Jid));
        }

        [Route("DisableJob/{Jid}")]
        [HttpPost]
        public IActionResult DisableJob([FromRoute] int Jid)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().DisablePost(URId, Jid));
        }

        [Route("JobType/Search")]
        [HttpGet]
        public IActionResult JobSearch([FromQuery]string keyword)
        {
            //var URId = HttpContext.Items["URId"];
            return Ok(new ER_JobDetails().JobTypeSearch(keyword));
        }
    }
}
