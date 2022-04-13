using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Cores.Common
{
    public class CustomOTPs
    {
        public string GenerateOTP()
        {
            string otp = string.Empty;
            string numbers = "0123456789";
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                int tempval = random.Next(0, numbers.Length);
                otp += tempval;
            }
            return otp;
        } 
    }
}
