using HIsabKaro.Cores.Employee.Profile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Profile
{
    [Route("Employee/Profile")]
    [ApiController]
    public class UpdateProfilesController : ControllerBase
    {
        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Update([FromRoute]int Id,Models.Employee.Resume.PersonalInfo value) 
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new UpdateProfiles().Update(UId,Id,value));
        }
    }
}
