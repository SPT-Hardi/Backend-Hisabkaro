using HIsabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class OtherCertificatesController : ControllerBase
    {
       
        [HttpPost]
        [Route("OtherCertificates")]
        public IActionResult Post(Models.Employee.Resume.OtherCertificate value)
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().Add(UID,value));
        }

        [HttpGet]
        [Route("OtherCertificates")]
        public IActionResult Get()
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().View(UID));
        }

        [HttpPatch]
        [Route("OtherCertificates/{Id}")]
        public IActionResult Patch([FromBody] Models.Employee.Resume.OtherCertificateDetails value, [FromRoute] int Id)
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().Update(Id, UID,value));
        }

        [HttpPost]
        [Route("OtherCertificates/Upload/{Id}")]
        public IActionResult PostCertificate([FromBody] Models.Employee.Resume.Certificate value,[FromRoute] int Id)
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().UploadCertificate(Id,UID,value));
        }

        [HttpDelete]
        [Route("OtherCertificates/{Id}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            int UId = (int)HttpContext.Items["UserID"];
            return Ok(new OtherCertificates().Delete(UId,Id));
        }
    }
}
