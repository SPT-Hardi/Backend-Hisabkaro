using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.MailService;
using MailChimp.Net;
using MailChimp.Net.Core;
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


        private const string ApiKey = "183ba0c13aaa5c0f40af22ae90f5fa9c-us14";
        private const string ListId = "26ba1dcc87";
        private const int TemplateId = 10018625; // (your template id) 10018625
        private MailChimpManager _mailChimpManager = new MailChimpManager(ApiKey);


        //public Models.Common.Result MailChimpCreate(MailRequest value)
        //{
        //    //===== MailCjimp.Net.V3
        //    MailChimpManager manager = new MailChimpManager(apiKey: "183ba0c13aaa5c0f40af22ae90f5fa9c-us14");
        //    var listId = "26ba1dcc87";
        //    var TemplateId = "10018625";//"174166181_41c478d636b04bf0074f_us14"; //10018625

        //    var r = manager.Templates.GetAllAsync().Result.ToList();
        //    var rr = manager.Templates.GetDefaultContentAsync(TemplateId).Result;

        //    return new Models.Common.Result()
        //    {
        //        Status = Models.Common.Result.ResultStatus.success,
        //        Message = string.Format($"Mail Send Successfully"),
        //        Data = new { r, rr },
        //    };
        //}

        // `html` contains the content of your email using html notation
        public Models.Common.Result CreateAndSendCampaign(string html)
        {    
            Setting _campaignSettings = new Setting
            {                  
                ToName="mayurpat@otobit.in",
                FromName = "Test",
                Title = "10",
                SubjectLine = "Template",
                TemplateId= TemplateId,
                Authenticate=true,
            };
            //Recipient _recipient = new Recipient
            //{
            //    SegmentOptions = new SegmentOptions
            //    {
            //        Conditions = new Condition
            //        {
            //            Operator = new Operator { }
            //        }
            //    }


            //};


            var campaign = _mailChimpManager.Campaigns.AddAsync(new Campaign
            {
                Settings = _campaignSettings,
                Recipients =new Recipient { ListId = ListId },
                Type = CampaignType.Regular ,
                Status="Sent",
            }).Result;

            var timeStr = DateTime.Now.ToString();

            var content = _mailChimpManager.Content.AddOrUpdateAsync(
                campaign.Id,
                new ContentRequest()
                {
                    Template = new ContentTemplate
                    {
                        Id = TemplateId,
                        Sections = new Dictionary<string, object>()
                        {
                            { "body_content", html },
                            { "preheader_leftcol_content", $"<p>{timeStr}</p>" }
                        }
                    }
                }).Result;

            _mailChimpManager.Campaigns.SendAsync(campaign.Id);// _mailChimpManager.Campaigns.SendAsync(campaign.Id).Wait();

            return new Models.Common.Result()
            {
                Status = Models.Common.Result.ResultStatus.success,
                Message = string.Format($"Mail Send Successfully"),
            };

        }



        public Models.Common.Result MailChimpCreate(MailRequest value)
        {
            CampaignRequest campaign = new CampaignRequest {ListId= ListId,Status=CampaignStatus.Sent };
            var r = _mailChimpManager.Campaigns.GetAllAsync(campaign).Result.ToList();
            //===== MailCjimp.Net.V3
            //MailChimpManager manager = new MailChimpManager(apiKey: "183ba0c13aaa5c0f40af22ae90f5fa9c-us14");
            //var listId = "26ba1dcc87";
            //var TemplateId = "174166181_41c478d636b04bf0074f_us14";

            //View ALL 
            //var mailChimpListCollection = manager.Lists.GetAllAsync().ConfigureAwait(false);

            //Add Member
            var member = new Member { EmailAddress = value.ToEmail, Status = Status.Subscribed };
            //member.MergeFields.Add("FNAME", "HOLY");
            //member.MergeFields.Add("LNAME", "COW");
            //_mailChimpManager.Members.AddOrUpdateAsync(ListId, member);

            //===== WebClient
            //var subscribeRequest = new
            //{
            //    apikey = "183ba0c13aaa5c0f40af22ae90f5fa9c-us14",
            //    id = "26ba1dcc87",
            //    email = new
            //    {
            //        email = value.ToEmail
            //    },
            //    double_optin = true,
            //};

            //    var requestJson = JsonConvert.SerializeObject(subscribeRequest);

            //    //var responseString = CallMailChimpApi("lists/subscribe.json", requestJson);
            //    //dynamic responseObject = JsonConvert.DeserializeObject(responseString);

            //    //if ((responseObject.email != null) && (responseObject.euid != null))
            //    //{
            //    //    // successful!
            //    //}
            //    //else
            //    //{
            //    //    string name = responseObject.name;
            //    //    if (name == "List_AlreadySubscribed")
            //    //    {
            //    //        Trace.TraceInformation("Mailchimp already subscribed");
            //    //    }
            //    //    else
            //    //    {
            //    //        Trace.TraceError("Mailchimp subscribe error {0}", responseObject.error);
            //    //    }
            //    //}
            return new Models.Common.Result()
            {
                Status = Models.Common.Result.ResultStatus.success,
                Message = string.Format($"Mail Send Successfully"),
                Data=r,
            };
        }
        //private static string CallMailChimpApi(string method, string requestJson)
        //{
        //    var endpoint = String.Format("https://{0}.api.mailchimp.com/2.0/{1}", "us14", method);
        //    var wc = new WebClient();
        //    try
        //    {
        //        return wc.UploadString(endpoint, requestJson);
        //    }
        //    catch (WebException we)
        //    {
        //        using (var sr = new StreamReader(we.Response.GetResponseStream()))
        //        {
        //            return sr.ReadToEnd();
        //        }
        //    }
        //}
    }
}

