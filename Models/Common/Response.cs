using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common
{
    public class Response
    {
        public class Data
        {
            public List<Dictionary<string, object>> Records { get; set; } = new List<Dictionary<string, object>>();
            public int Total { get; set; } = 0;
            public int CurrentTotal { get; set; } = 0;
            public string Filter { get; set; } = "";
            public string Sort { get; set; } = "";
            public int PageSize { get; set; } = 0;
            public int PageNumber { get; set; } = 0;
        }
    }

}
