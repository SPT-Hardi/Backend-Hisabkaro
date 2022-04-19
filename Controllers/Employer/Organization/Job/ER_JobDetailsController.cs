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
            int URId = (int)HttpContext.Items["URId"];
            //int Uid = 50000001;
            //int Rid = 1000004;
            return Ok( _jobDetails.Create(URId,value));
        }

        [Route("Job/{Jid}")]
        [HttpPut]
        public IActionResult Update(int Jid,Models.Employer.Organization.Job.ER_JobDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok( _jobDetails.Update(URId, Jid,value));
        }

        [Route("OrgJob")]
        [HttpGet]
        public IActionResult Get()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok( _jobDetails.One(URId));
        }

        [Route("Job/{Jid}")]
        [HttpGet]
        public IActionResult GetById([FromRoute]int Jid)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok( _jobDetails.GetJob(URId,Jid));
        }

        [Route("RemoveJob/{Jid}")]
        [HttpPost]
        public IActionResult Remove([FromRoute]int Jid)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(_jobDetails.RemovePost(URId, Jid));
        }

        [Route("DisableJob/{Jid}")]
        [HttpPost]
        public IActionResult DisableJob([FromRoute] int Jid)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(_jobDetails.DisablePost(URId, Jid));
        }
    }
}
