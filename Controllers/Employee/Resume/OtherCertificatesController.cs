using HisaabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class OtherCertificatesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public OtherCertificatesController(IWebHostEnvironment Environment) 
        {
            _environment = Environment;
        }
        [HttpPost]
        [Route("OtherCertificates")]
        public IActionResult Post(Models.Employee.Resume.OtherCertificate value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().Add(value,UID.ToString()));
        }

        [HttpGet]
        [Route("OtherCertificates")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().View(UID.ToString()));
        }

        [HttpPatch]
        [Route("OtherCertificates/{Id}")]
        public IActionResult Patch([FromBody]Models.Employee.Resume.OtherCertificateDetails value,[FromRoute]int Id)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().Update(value,Id,UID.ToString()));
        }

        [HttpPost]
        [Route("OtherCertificates/Upload/{Id}")]
        public IActionResult PostCertificate([FromForm]Models.Common.File.Upload objFile,int Id)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().UploadCertificate(objFile, Id,UID.ToString(),_environment));
        }
    }
}
