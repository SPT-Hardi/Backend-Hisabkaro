using HIsabKaro.Cores.Employee.Resume;
using HIsabKaro.Models.Employee.Resume;
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
    public class ContactsController : ControllerBase
    {
        [HttpGet]
        [Route("Contact")]
        public IActionResult Get() 
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Contacts().Get(UID));
        }

        [HttpPatch]
        [Route("Contact")]
        public IActionResult Patch(Contact value)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Contacts().Update(UID,value));
        }
    }
}
