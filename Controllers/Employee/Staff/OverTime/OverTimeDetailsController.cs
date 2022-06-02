using HIsabKaro.Cores.Employee.Staff.OverTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Staff.OverTime
{
    [Route("Employee/Staff/OverTime")]
    [ApiController]
    public class OverTimeDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("OverTimeDetails/One/{Date}")]
        public IActionResult OverTime([FromRoute] DateTime Date)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new OverTimeDetails().One(URId, Date));
        }
    }
}
