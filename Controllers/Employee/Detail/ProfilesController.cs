using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Employee.Details
{
    [Route("Employee/Details")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        [HttpGet]
        [Route("Profile/Address")]
        public IActionResult AddressgetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/Address")]
        public IActionResult AddressPost() 
        {
            return Ok();
        }
        [HttpGet]
        [Route("Profile/Education")]
        public IActionResult EducationgetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/Education")]
        public IActionResult EducationPost()
        {
            return Ok();
        }
        [HttpGet]
        [Route("Profile/Identity")]
        public IActionResult IdentitygetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/Identity")]
        public IActionResult IdentityPost()
        {
            return Ok();
        }
        [HttpGet]
        [Route("Profile/OtherCertificate")]
        public IActionResult OtherCertificategetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/OtherCertificate")]
        public IActionResult OtherCertificatePost()
        {
            return Ok();
        }
        [HttpGet]
        [Route("Profile/Skills")]
        public IActionResult SkillsgetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/Skills")]
        public IActionResult SkillsPost()
        {
            return Ok();
        }
        [HttpGet]
        [Route("Profile/WorkExperience")]
        public IActionResult WorkExperiencegetPost()
        {
            return Ok();
        }
        [HttpPost]
        [Route("Profile/WorkExperience")]
        public IActionResult WorkExperiencePost()
        {
            return Ok();
        }
    }
}
