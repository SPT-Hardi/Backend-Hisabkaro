using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common
{
    public class CustomeDrop
    {
        public class Org_Branch_Drop 
        {
            public IntegerNullString Organization { get; set; }
            public List<IntegerNullString> Branches { get; set; }
        }
        public class Shit_Time 
        {
            [JsonIgnore]
            public int ShiftTimeId { get; set; }
            public TimeSpan? StartTime { get; set; }
            public TimeSpan? EndTime { get; set; }
            public TimeSpan? MarkLate { get; set; }
        }
    }
}
