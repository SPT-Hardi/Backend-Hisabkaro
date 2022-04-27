using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class SubmitDaily
    {
        public DateTime CheckIN { get; set; }

    }
    public class SubmitDailyThroughQR 
    {
        [Required]
        public string QRString { get; set; }
        public DateTime CheckIN { get; set; }
        [Required]
        public string FGUID { get; set; }
    }
}
