using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Services
{
    [Route("service/AttendanceReport")]
    [ApiController]
    public class AttendanceReportController : ControllerBase
    {
        private readonly IConverter _converter;

        public AttendanceReportController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpGet]
        [Route("Get")]
        public IActionResult Get([FromQuery]int URId,[FromQuery]DateTime startDate,[FromQuery]DateTime endDate) 
        {
            var FGUID = Guid.NewGuid();
            /*var URId = HttpContext.Items["URId"];
            if (URId == null) 
            {
                throw new ArgumentException("Token not vaild or expired!");
            }*/
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10 },
                DocumentTitle = "PDF Report",
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
               // HtmlContent = HIsabKaro.Services.PDF.HTMLString.GetHTMLStringForAttendanceReport(URId,startDate,endDate),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "AttendanceReport.css")},
                HeaderSettings = { FontName = "Arial", FontSize = 9/*, Right = "Page [page] of [toPage]", Line = true ,*/ },
                FooterSettings = { FontName = "Arial", FontSize = 9 /*Line = true, Center = "Report Footer" */}
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", $"{FGUID}.pdf");
        }
    }
}
