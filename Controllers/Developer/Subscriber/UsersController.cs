using HIsabKaro.Cores.Developer.Subscriber;
using HIsabKaro.Models.Developer.Subscriber;
using HIsabKaro.Services;
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
            return Ok(new Users(_configuration, _tokenServices).VerifyOtp(value));
        }

        [HttpPost]
        [Route("User/Account/LoginType")]
        public IActionResult Default_Login_Type(DefaultLoginType value)
        {
            var UId = HttpContext.Items["UId"];
            var DeviceToken = HttpContext.Items["DeviceToken"];
            return Ok(new Users(_configuration, _tokenServices).Select_LoginType(UId,DeviceToken,value));
        }

    }
}
