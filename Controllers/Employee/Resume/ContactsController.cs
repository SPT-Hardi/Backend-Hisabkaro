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
            int UId = (int)HttpContext.Items["UserID"];
            return Ok(new Contacts().Get(UId));
        }

        [HttpPatch]
        [Route("Contact")]
        public IActionResult Patch(Contact value)
        {
            int UId = (int)HttpContext.Items["UserID"];
            return Ok(new Contacts().Update(UId,value));
        }
    }
}
