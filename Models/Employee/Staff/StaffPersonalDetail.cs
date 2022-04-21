﻿using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.Contact;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Staff
{
    public class StaffPersonalDetail
    {
        public DateTime DOB { get; set; }
        public string Gender { get; set; } 
        public Address Address { get; set; }                   
    }
}