using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Common.File
{
    public class Upload
    {
        public IFormFile files { get; set; }
    }
}
