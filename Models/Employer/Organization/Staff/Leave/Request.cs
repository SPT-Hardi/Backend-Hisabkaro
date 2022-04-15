using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Leave
{
    public class Request
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string reason { get; set; }

        public bool Ispaid { get; set; }
    }
}