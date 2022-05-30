using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Leave
{
    public class Request
    {
        [Required(ErrorMessage ="StartDate is required!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage ="EndDate is required!")]
        public DateTime EndDate { get; set; }

        public string reason { get; set; }
    }
}