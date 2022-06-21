using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Services
{
    [Route("Services/ResumePDF")]
    [ApiController]
    public class ResumePDFController : ControllerBase
    {
        private readonly IConverter _converter;

        public ResumePDFController(IConverter converter) 
        {
            _converter = converter;
        }
        [HttpGet]
        [Route("Get/{Id}")]
        public IActionResult CreatePDF(int Id)
        {
            var FGUID = Guid.NewGuid();
            //var UId = HttpContext.Items["UserID"];
            //var filePath = $@"D:\BHK\Backend-Hisabkaro\PDFs\{FGUID}.pdf";
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom=10 },
                DocumentTitle = "PDF Report",
                //Out = filePath
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = HIsabKaro.Services.PDF.HTMLString.GetHTMLStringForResume(Id),
                WebSettings = { DefaultEncoding = "utf-8" , UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9 /*, Right = "Page [page] of [toPage]", Line = true ,*/ },
                FooterSettings = { FontName = "Arial", FontSize = 9 /*Line = true, Center = "Report Footer" */}
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file =_converter.Convert(pdf);
            /*var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyToAsync(memory);
            }
            memory.Position = 0;*/
           return File(file, "application/pdf",$"{FGUID}.pdf");
           // return File(memory, GetContentType(filePath), filePath);
        }

     
    /*    [HttpGet]
      public string GetContentType(string path)
      {
          var provider = new FileExtensionContentTypeProvider();
          string contentType;
      
          if (!provider.TryGetContentType(path, out contentType))
          {
              contentType = "application/octet-stream";
          }
          return contentType;
      }*/
    }
}
