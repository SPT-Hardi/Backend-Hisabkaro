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
            int UID = (int)HttpContext.Items["UserID"];
            string DeviceToken= HttpContext.Items["DeviceToken"].ToString();
            return Ok(new UserDetails(_configuration,_tokenServices).Add(UID,DeviceToken.ToString(),value));
        }


    }
}
