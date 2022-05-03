using HIsabKaro.Cores.Common.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common.File
{
    [Route("Common/File")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        
        private readonly IWebHostEnvironment _environment;

        public UploadsController(IWebHostEnvironment Environment)
        {
           
            _environment = Environment;
        }

        [Route("Upload")]
        [HttpPost]
        public IActionResult Create([FromForm]Models.Common.File.Upload files)
        {
            return Ok(new Uploads(_environment).Upload(files));
        }

        [Route("BulkStaffDetail")]
        [HttpPost]
        public IActionResult BulkCreate([FromForm] Models.Common.File.Upload files)
        {
            return Ok(new Uploads(_environment).BulkCreate(files));
        }
    }
}
