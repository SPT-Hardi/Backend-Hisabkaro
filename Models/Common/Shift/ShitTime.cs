using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common.Shift
{
    public class ShitTime
    {
        public List<TimeList> TimeLists { get; set; } = new List<TimeList>();
    }
    public class TimeList
    {
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public TimeSpan? MarkLate { get; set; }
    }
}
