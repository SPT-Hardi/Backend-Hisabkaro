﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common
{
    public class ISDT
    {
        public DateTime GetISDT(DateTime datetime) 
        {
             TimeZoneInfo INDIAN_ZONE;
             var name = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
             if (System.Diagnostics.Process.GetCurrentProcess().ProcessName== "w3wp")
             {
               INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
             }
            else
            {
               INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");

            }
             DateTime ISTDate = TimeZoneInfo.ConvertTimeFromUtc(datetime.ToUniversalTime(), INDIAN_ZONE);
             return ISTDate;
        }
    }
}
