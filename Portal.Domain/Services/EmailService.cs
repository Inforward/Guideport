using Portal.Infrastructure.Helpers;
using Portal.Infrastructure.Logging;
using Portal.Model;
using Portal.Services.Contracts;
using System;
using System.Net.Mail;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace Portal.Domain.Services
{
    public class EmailService : IEmailService
    {
        private const string REGEX_HTML = @"\<(?<Tag>[\w]+).*\>.*\<\/(?<TagEnd>[\w]+)\>";
        private readonly ILogService _logService;

        public EmailService(ILogService logService)
        {
            _logService = logService;
        }

        public void Send(Email email)
        {
            Required.NotNull(email, "Email");
            Required.NotNullOrEmpty(email.To, string.Empty, "To");
            Required.NotNullOrEmpty(email.From, string.Empty, "From");
            Required.NotNullOrEmpty(email.Subject, string.Empty, "Subject");
            Required.NotNullOrEmpty(email.Body, string.Empty, "Body");

            var mailMessage = new MailMessage();
            var smtpClient = new SmtpClient();

            try
            {
                if (!string.IsNullOrEmpty(email.From))
                    mailMessage.From = new MailAddress(email.From);

                if (!string.IsNullOrEmpty(email.To))
                    email.To.Split(';', ',').ForEach(address => { if (!string.IsNullOrEmpty(address)) { mailMessage.To.Add(address); } });

                if (!string.IsNullOrEmpty(email.Cc))
                    email.Cc.Split(';', ',').ForEach(address => { if (!string.IsNullOrEmpty(address)) { mailMessage.CC.Add(address); } });

                if (!string.IsNullOrEmpty(email.Bcc))
                    email.Bcc.Split(';', ',').ForEach(address => { if (!string.IsNullOrEmpty(address)) { mailMessage.Bcc.Add(address); } });

                var re = new Regex(REGEX_HTML,
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase |
                    RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);

                mailMessage.Subject = email.Subject;
                mailMessage.Body = email.Body;
                mailMessage.IsBodyHtml = re.IsMatch(email.Body);

                smtpClient.Send(mailMessage);

                Log(email, "Email successfully sent.");
            }
            catch (Exception ex)
            {
                var message = "Email send failed.";

                if (ex is SmtpFailedRecipientException)
                    message += string.Format("  Recipient: {0}.", (ex as SmtpFailedRecipientException).FailedRecipient);

                Log(email, message, ex);
            }
        }

        private void Log(Email email, string message, Exception ex = null)
        {
            var log = new EventLog()
            {
                EventTypeID = ex != null ? (byte)EventTypes.Error : (byte)EventTypes.Information,
                EventDate = DateTime.Now,
                Message = message,
                PostData = email.Body,
                ServerName = Environment.MachineName,
                Source = string.Format("Email Info - From: {0}, To: {1}, Cc: {2}, Bcc: {3}, Subject: {4}", email.From, email.To, email.Cc, email.Bcc, email.Subject)
            };

            if (OperationContext.Current != null)
            {
                var action = OperationContext.Current.IncomingMessageHeaders.Action;

                log.RequestMethod = action.Substring(action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
                log.ScriptName = OperationContext.Current.IncomingMessageHeaders.To.AbsoluteUri;
            }

            if (ex != null)
            {
                log.ErrorText = ex.FullMessage();
                log.StackTrace = ex.StackTrace;
            }


            _logService.LogEvent(log);

        }
    }
}
