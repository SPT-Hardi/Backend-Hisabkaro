using HIsabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class WorkExperiencesController : ControllerBase
    {
        [HttpPost]
        [Route("WorkExperiences")]
        public IActionResult Post(Models.Employee.Resume.WorkExperinece value) 
        {
           int UID = (int)HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().Add(UID,value));
        }

        [HttpGet]
        [Route("WorkExperiences")]
        public IActionResult Get()
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().View(UID));
        }

        [HttpPatch]
        [Route("WorkExperiences/{Id}")]
        public IActionResult Patch(Models.Employee.Resume.WorkExperienceDetails value,int Id)
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new WorkExperiences().Update(Id, UID,value));
        }

       
    }
}
