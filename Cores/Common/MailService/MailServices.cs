using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.MailService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Result Create(MailRequest mailRequest)
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

            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = string.Format($"{email.Sender} send a mail to {email.To}"),
            };
        }
    }
}
