using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.MailService;
using MailChimp.Net;
using MailChimp.Net.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common.MailService
{
    public class MailServices
    {
        private readonly IOptions<MailSetting> _mailSetting;

        public MailServices(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting;
        }
        public Models.Common.Result Create(MailRequest mailRequest)
        {      
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Value.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Value.Host, _mailSetting.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Value.Mail, _mailSetting.Value.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

            return new Models.Common.Result()
            {
                Status = Models.Common.Result.ResultStatus.success,
                Message = string.Format($"{email.Sender} send a mail to {email.To}"),
            };
        }

        public Models.Common.Result MailChimpCreate(MailRequest value)
        {
            //===== MailCjimp.Net.V3
            MailChimpManager manager = new MailChimpManager(apiKey: "183ba0c13aaa5c0f40af22ae90f5fa9c-us14");
            var listId = "26ba1dcc87";

            //View ALL 
            //var mailChimpListCollection = manager.Lists.GetAllAsync().ConfigureAwait(false);

            //Add Member
            var member = new Member { EmailAddress = value.ToEmail, Status = Status.Subscribed };
            //member.MergeFields.Add("FNAME", "HOLY");
            //member.MergeFields.Add("LNAME", "COW");
            manager.Members.AddOrUpdateAsync(listId, member);

            //===== WebClient
            var subscribeRequest = new
            {
                apikey = "183ba0c13aaa5c0f40af22ae90f5fa9c-us14",
                id = "26ba1dcc87",
                email = new
                {
                    email = value.ToEmail
                },
                double_optin = true,
            };

            var requestJson = JsonConvert.SerializeObject(subscribeRequest);

            //var responseString = CallMailChimpApi("lists/subscribe.json", requestJson);
            //dynamic responseObject = JsonConvert.DeserializeObject(responseString);

            //if ((responseObject.email != null) && (responseObject.euid != null))
            //{
            //    // successful!
            //}
            //else
            //{
            //    string name = responseObject.name;
            //    if (name == "List_AlreadySubscribed")
            //    {
            //        Trace.TraceInformation("Mailchimp already subscribed");
            //    }
            //    else
            //    {
            //        Trace.TraceError("Mailchimp subscribe error {0}", responseObject.error);
            //    }
            //}
            return new Models.Common.Result()
            {
                Status = Models.Common.Result.ResultStatus.success,
                Message = string.Format($"Mail Send Successfully"),
            };
        }
        private static string CallMailChimpApi(string method, string requestJson)
        {
            var endpoint = String.Format("https://{0}.api.mailchimp.com/2.0/{1}", "us14", method);
            var wc = new WebClient();
            try
            {
                return wc.UploadString(endpoint, requestJson);
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
