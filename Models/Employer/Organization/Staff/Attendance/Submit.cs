using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class Submit
    {
        [JsonIgnore]
        public int EmprAttendanceDailyId { get; set; }
        public DateTime CheckIN { get; set; }

        public int RId { get; set; }
    }
}
