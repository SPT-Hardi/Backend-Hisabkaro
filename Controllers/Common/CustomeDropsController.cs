using HIsabKaro.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Route("Common/CustomDrop")]
    [ApiController]
    public class CustomeDropsController : ControllerBase
    {
        [HttpGet]
        [Route("General/Data/LoginType")]
        public IActionResult Get() 
        {
            return Ok(new CustomeDrops.RoleDrop().Get());
        }
    }
}
