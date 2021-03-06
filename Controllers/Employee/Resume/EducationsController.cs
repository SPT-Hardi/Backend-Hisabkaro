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
    public class EducationsController : ControllerBase
    {
        [HttpPost]
        [Route("Eductions")]
        public IActionResult Post([FromBody]Models.Employee.Resume.Education value)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Educations().Add(UID.ToString(),value));
        }
        [HttpGet]
        [Route("Educations")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UserID"];
            if (UID == null) 
            {
                throw new ArgumentException("Not authorized!,(enter valid token)");
            }
            return Ok(new Educations().View(UID.ToString()));
        }
        [HttpPatch]
        [Route("Educations/{Id}")]
        public IActionResult Patch([FromBody]Models.Employee.Resume.EducationDetail value,[FromRoute]int Id) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Educations().Update(Id, UID.ToString(),value));
        }
    }
}
