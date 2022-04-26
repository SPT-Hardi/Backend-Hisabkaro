﻿using HIsabKaro.Models.Common.Contact;
using HIsabKaro.Models.Common.Shift;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Branch
{
    public class BranchDetail
    {
        public Models.Common.IntegerNullString Organization { get; set; } = new Models.Common.IntegerNullString();

        [Required(ErrorMessage = "Branch name is required")]
        public string BranchName { get; set; }

        public bool status { get; set; }

        public List<ShitTime> ShitTime { get; set; } = new List<ShitTime>();

        public Address Address { get; set; }

        public decimal longitude { get; set; }

        public decimal latitude { get; set; }

    }
}
