using System;
using System.Collections.Generic;
using System.Linq;
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
            public int ShiftTimeId { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }
    }
}
