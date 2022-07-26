using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Attendance
{
    [Route("Employer/Organization/Staff/Attendance")]
    [ApiController]
    public class HistoryByDaysController : ControllerBase
    {
        /*[HttpGet]
        [Route("HistoyByDay")]
        public IActionResult Get([FromQuery]DateTime date) 
        {
            var URId = HttpContext.Items["URId"];
            if (URId == null) 
            {
                throw new ArgumentException("Token Expired or not valid!");
            }
            return Ok(new HistoryByDays().Get(URId,date));
        }*/

    }
}
