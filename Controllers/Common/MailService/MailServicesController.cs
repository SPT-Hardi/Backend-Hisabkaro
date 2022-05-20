using HIsabKaro.Cores.Common.MailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common.MailService
{
    [Route("Common/MailService")]
    [ApiController]
    public class MailServicesController : ControllerBase
    {
        private readonly MailServices _mailServices;
        public MailServicesController(MailServices mailServices)
        {
            _mailServices = mailServices;
        }
        [HttpPost]
        [Route("MailServices/Create")]
        public IActionResult Create([FromBody]Models.Common.MailService.MailRequest value)
        {
            return Ok(_mailServices.Create(value));
        }

        [HttpPost]
        [Route("MailServices/MailChimp/Create")]
        public IActionResult MailChimpCreate([FromBody] Models.Common.MailService.MailRequest value)
        {
            return Ok(_mailServices.MailChimpCreate(value));
        }
    }
}
