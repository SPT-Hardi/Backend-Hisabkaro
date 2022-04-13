using HisaabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class WorkExperiencesController : ControllerBase
    {
        [HttpPost]
        [Route("WorkExperiences")]
        public IActionResult Post(Models.Employee.Resume.WorkExperinece value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().Add(value,UID.ToString()));
        }

        [HttpGet]
        [Route("WorkExperiences")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().View(UID.ToString()));
        }

        [HttpPatch]
        [Route("WorkExperiences/{Id}")]
        public IActionResult Patch(Models.Employee.Resume.WorkExperienceDetails value,int Id)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().Update(value,Id,UID.ToString()));
        }

       
    }
}
