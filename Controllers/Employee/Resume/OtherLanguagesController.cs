using HIsabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Resume
{
    [Route("Emploee/Resume/OtherLanguages")]
    [ApiController]
    public class OtherLanguagesController : ControllerBase
    {
        [HttpGet]
        [Route("View")]
        public IActionResult View() 
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new OtherLanguages().View(UId));
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody]Models.Employee.Resume.List_OtherLanguages value)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new OtherLanguages().Update(UId,value));
        }
    }
}
