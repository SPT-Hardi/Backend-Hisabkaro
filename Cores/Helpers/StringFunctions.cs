using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace HIsabKaro.Cores.Helpers
{
    public partial class StringFunctions
    {
        public static string FullName(string FirstName, string MiddleName, string LastName)
        {
            if (FirstName is null)
                FirstName = "";
            if (MiddleName is null)
                MiddleName = "";
            if (LastName is null)
                LastName = "";
            return ((FirstName + " " + MiddleName).Trim() + " " + LastName).Trim();
        }

        public static string UniqueKeyViolation(string message)
        {
            return Regex.Match(message, "(?<=UQ)(.*)(?=UQ)").Value.Replace("_", " ") + " : " + Regex.Match(message, @"(?<=\()(.*)(?=\))").Value.Split(',')[0] + " already exists!";
        }

        public static string SplitCamelCase(string str)
        {
            return Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        }

        public static string GetStringBetween(string StartString, string EndString, string SearchString)
        {
            return Regex.Match(SearchString, "(?<=" + StartString + ")(.*)(?=" + EndString + ")").Value;
        }

        public static string EncryptData(string plaintext)
        {
            var plaintextBytes = Encoding.Unicode.GetBytes(plaintext);
            var ms = new MemoryStream();
            var encStream = new CryptoStream(ms, TripleDES.Create().CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
            encStream.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptData(string encryptedtext)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedtext);
            var ms = new MemoryStream();
            var decStream = new CryptoStream(ms, TripleDES.Create().CreateDecryptor(), CryptoStreamMode.Write);
            decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
            decStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(ms.ToArray());
        }

        public static string GetOTP()
        {
            string OTPLength = "6";
            string NewCharacters = "";
            string allowedChars = "1,2,3,4,5,6,7,8,9,0";
            var sep = new[] { ',' };
            var arr = allowedChars.Split(sep);
            string IDString = "";
            string temp = "";
            var rand = new Random();
            for (int i = 0, loopTo = Convert.ToInt32(OTPLength) - 1; i <= loopTo; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                IDString += temp;
                NewCharacters = IDString;
            }

            return NewCharacters;
        }
    }
}