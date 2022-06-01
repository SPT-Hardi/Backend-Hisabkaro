using HIsabKaro.Cores.Developer.Subscriber;
using HIsabKaro.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Developer.Subscriber
{
    [Route("Developer/Subscriber")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        private readonly IWebHostEnvironment _environment;

        public UserDetailsController(IConfiguration configuration, ITokenServices tokenServices, IWebHostEnvironment Environment) 
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
            _environment = Environment;
        }
        [HttpPost]
        [Route("UserDetails")]
        public IActionResult DetailsPost(Models.Developer.Subscriber.UserDetails value) 
        {
            var UID = HttpContext.Items["UserID"];
            var DeviceToken= HttpContext.Items["DeviceToken"];
            return Ok(new UserDetails(_configuration,_tokenServices).Add(UID,DeviceToken,value));
        }

        [HttpGet]
        [Route("UserDetails")]
        public IActionResult DetailsGet()
        {
            var UID = HttpContext.Items["UserID"];
            //var DeviceToken = HttpContext.Items["DeviceToken"];
            return Ok(new UserDetails(_configuration, _tokenServices).Get(UID));
        }


        [HttpPatch]
        [Route("UserDetails")]
        public IActionResult DetailsPatch(Models.Developer.Subscriber.UserPersonalDetails value)
        {
            var UID = HttpContext.Items["UserID"];
            //var DeviceToken = HttpContext.Items["DeviceToken"];
            return Ok(new UserDetails(_configuration, _tokenServices).Update(UID,value));
        }

    }
}
