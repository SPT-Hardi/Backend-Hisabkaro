using HisaabKaro.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Common
{
    [Route("Common/FileUpload")]
    [ApiController]
    public class FileControllerController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FileControllerController(IWebHostEnvironment Environment) 
        {
            _environment = Environment;
        }
        [HttpPost]
        public IActionResult Upload([FromForm]Models.Common.File.Upload file) 
        {
            FileUploadServices fileUploadServices = new FileUploadServices(_environment);
            var res = fileUploadServices.Upload(file);
            return Ok(res);
        }
    }
}
