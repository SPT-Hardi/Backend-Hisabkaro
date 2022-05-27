using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common
{
    public class StringFormators
    {
        public string CountDuration(DateTime startdate,DateTime enddate) 
        {
            if (enddate < startdate) 
            {
                throw new ArgumentException("Enter valid daterange");
            }
            var totaldays = (enddate - startdate).TotalDays;
            var totalmonths = Math.Floor(totaldays / 30);
            var startdatestring = $"{startdate.ToString("MMMM").Substring(0, 3) } {startdate.Year}";//Apr 2022
            var enddatestring = $"{enddate.ToString("MMMM").Substring(0, 3) } {enddate.Year}";
            var durationstring = $"{Math.Floor(totalmonths / 12)}yr {Math.Floor(totalmonths % 12)}mon";
            var res = $"{startdatestring} - {enddatestring}";
            return res;
        }
    }
}
