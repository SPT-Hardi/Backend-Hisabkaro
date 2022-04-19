using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class Override
    {
        public int URId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public IntegerNullString Status { get; set; } = new IntegerNullString();
    }
}
