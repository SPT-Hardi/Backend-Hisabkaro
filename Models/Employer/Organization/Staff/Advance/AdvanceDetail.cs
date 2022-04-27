﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Advance
{
    public class AdvanceDetail
    {     
        public DateTime Date { get; set; }
        public Decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
