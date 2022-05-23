using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [ApiController]
    public class TestController : ControllerBase
    {
        [Route("test")]
        [HttpGet]
        public IActionResult Get() 
        {
            return Ok("Working!!");
        }
        [Route("test2")]
        [HttpGet]
        public IActionResult Get2()
        {
            return Ok("Working  :) !!");
        }
        [Route("test3")]
        [HttpGet]
        public IActionResult Get3()
        {
            return Ok("Working  :) !!");
        }
        [Route("put")]
        [HttpPut]
        public IActionResult GetPut()
        {
            return Ok(" PUT Working!!");
        }
    }
}
