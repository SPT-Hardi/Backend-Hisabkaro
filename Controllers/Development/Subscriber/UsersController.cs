using HisaabKaro.Cores.Development.Subscriber;
using HisaabKaro.Models.Developer.Subscriber;
using HisaabKaro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Development.Subscriber
{
    [Route("Development/Subscriber")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;

        public UsersController(IConfiguration configuration, ITokenServices tokenServices) 
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
        }
        [HttpPost]
        [Route("User/GenerateOTP")]
        public IActionResult otpPost(Models.Developer.Subscriber.User value) 
        {
           
            return Ok(new Users(_configuration,_tokenServices).Add(value));
        }

        [HttpPost]
        [Route("User/VerifyOTP")]
        public IActionResult otpverifyPost(UserMobile value)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Users(_configuration, _tokenServices).VerifyOtp(value));
        }
        [HttpPost]
        [Route("User/Role")]
        public IActionResult RolePost(Models.Developer.Subscriber.Role value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Users(_configuration, _tokenServices).AddRole(value, UID.ToString()));
        }

    }
}
