using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HIsabKaro.Services
{
    public class SMSServices
    {
        public byte[] Send(string mobilenumber,string otp)
        {

            String message = HttpUtility.UrlEncode($"Your otp for HisabKaro:{otp}");
            using (var wb = new WebClient())
            {
                byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                {
                {"apikey" , "NTg2NjRkNDk2ZDVhMzc3NjM3NjM2NDQ5NGQ0OTQ1NjQ="},
                {"numbers" , $"91{mobilenumber}"},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                string result = System.Text.Encoding.UTF8.GetString(response);
                return response;
            }
        }
        public string Get(string mobilenumber, string otp) 
        {
				String result;
				string apiKey = "NTg2NjRkNDk2ZDVhMzc3NjM3NjM2NDQ5NGQ0OTQ1NjQ=";
				string numbers = $"91{mobilenumber}"; // in a comma seperated list
				string message = $"Hi there, thank you for sending your first test message from Textlocal. Get 20% off today with our code: {otp}.";
				string sender = "600010";

				String url = "https://api.textlocal.in/send/?apikey=" + apiKey + "&numbers=" + numbers + "&message=" + message + "&sender=" + sender;
				//refer to parameters to complete correct url string

				StreamWriter myWriter = null;
				HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);

				objRequest.Method = "POST";
				objRequest.ContentLength = Encoding.UTF8.GetByteCount(url);
				objRequest.ContentType = "application/x-www-form-urlencoded";
				try
				{
					myWriter = new StreamWriter(objRequest.GetRequestStream());
					myWriter.Write(url);
				}
				catch (Exception e)
				{
					return e.Message;
				}
				finally
				{
					myWriter.Close();
				}

				HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
				using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
				{
					result = sr.ReadToEnd();
					// Close and clean up the StreamReader
					sr.Close();
				}
				return result;
			
		}
    }
}
