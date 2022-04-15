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
        private readonly Uploads _images;
        private readonly ER_JobDetails _jobDetails;

        public ER_JobDetailsController(Uploads images,ER_JobDetails jobDetails)
        {
            _images = images;
            _jobDetails = jobDetails;
        }

        [Route("Job")]
        [HttpPost]
        public IActionResult Create(Models.Employer.Organization.Job.ER_JobDetail value)
        {
            int Uid = 1;
            return Ok( _jobDetails.Create(Uid,value));
        }

        [Route("Job/{Jid}")]
        [HttpPut]
        public IActionResult Update(Models.Employer.Organization.Job.ER_JobDetail value,int Jid)
        {
            int Uid = 1;
            return Ok( _jobDetails.Update(Uid, Jid,value));
        }

        [Route("OrgJob/{OId}")]
        [HttpGet]
        public IActionResult Get([FromRoute]int OId)
        {
            return Ok( _jobDetails.One(OId));
        }

        [Route("Job/{Jid}")]
        [HttpGet]
        public IActionResult GetById([FromRoute]int Jid)
        {
            return Ok( _jobDetails.GetJob(Jid));
        }

        [Route("RemoveJob/{Jid}")]
        [HttpPost]
        public IActionResult Remove([FromRoute]int Jid)
        {
            return Ok(_jobDetails.RemovePost(Jid));
        }

        [Route("DisableJob/{Jid}")]
        [HttpPost]
        public IActionResult DisableJob([FromRoute] int Jid)
        {
            return Ok(_jobDetails.DisablePost(Jid));
        }
    }
}
