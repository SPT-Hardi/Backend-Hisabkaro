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
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new Educations().Add(UID,value));
        }
        [HttpGet]
        [Route("Educations")]
        public IActionResult Get()
        {
            int UID = (int)HttpContext.Items["UserID"];
            if (UID == 0) 
            {
                throw new ArgumentException("Not authorized!,(enter valid token)");
            }
            return Ok(new Educations().View(UID));
        }
        [HttpPatch]
        [Route("Educations/{Id}")]
        public IActionResult Patch([FromBody]Models.Employee.Resume.EducationDetail value,[FromRoute]int Id) 
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new Educations().Update(Id, UID,value));
        }

        [HttpDelete]
        [Route("Educations/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            int UId = (int)HttpContext.Items["UserID"];
            return Ok(new Educations().Delete(UId,Id));
        }
    }
}
