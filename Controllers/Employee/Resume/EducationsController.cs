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
        /*[HttpPost]
        [Route("Eductions")]
        public IActionResult Post([FromBody]Models.Employee.Resume.Education value)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Educations().Add(UID,value));
        }*/

        [HttpGet]
        [Route("Educations")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Educations().View(UID));
        }

        [HttpPatch]
        [Route("Educations")]
        public IActionResult Patch([FromBody] Models.Employee.Resume.Educations value)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Educations().Update(UID, value));
        }

        /*  [HttpDelete]
          [Route("Educations/{Id}")]
          public IActionResult Delete([FromRoute] int Id)
          {
              var UID = HttpContext.Items["UId"];
              return Ok(new Educations().Delete(UID,Id));
          }*/
    }
}
