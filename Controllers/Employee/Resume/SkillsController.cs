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
    public class SkillsController : ControllerBase
    {
  /*      [HttpPost]
        [Route("Skills")]
        public IActionResult Add(Models.Employee.Resume.Skill value) 
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Skills().Add(UID,value));
        }*/

        [HttpGet]
        [Route("Skills")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Skills().View(UID));
        }

        [HttpPatch]
        [Route("Skills")]
        public IActionResult Update(Models.Employee.Resume.List_Skills value)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Skills().Update(UID,value));
        }

        /*[HttpDelete]
        [Route("Skills/{Id}")]
        public IActionResult Update([FromRoute]int Id)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Skills().Delete(UID,Id));
        }*/
    }
}
