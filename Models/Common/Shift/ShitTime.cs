using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common.Shift
{
    public class ShitTime
    {
        [JsonIgnore]
        public int ? ShiftTimeId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? MarkLate { get; set; }
    }
}
