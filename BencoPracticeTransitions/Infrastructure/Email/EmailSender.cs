using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BencoPracticeTransitions.Tests")]

namespace BencoPracticeTransitions.Infrastructure.Email
{
    public class EmailSender : ISendEmail
    {
        private readonly BencoSmtpSettings _bencoSmtpSettings;
        public EmailSender(IOptions<BencoSmtpSettings> bencoMailSettings)
        {
            _bencoSmtpSettings = bencoMailSettings.Value;
        }

        public void Send(EmailRequest emailRequest)
        {
            using (var client = new SmtpClient(_bencoSmtpSettings.Host, _bencoSmtpSettings.Port)
            {
                Credentials = new NetworkCredential(_bencoSmtpSettings.User, _bencoSmtpSettings.Password)
            })
            {
                var mail = CreateEmail(emailRequest);
                client.Send(mail);
            }
        }

        internal MailMessage CreateEmail(EmailRequest emailRequest)
        {
            if (emailRequest == null)
            {
                throw new ArgumentNullException(nameof(emailRequest));
            }

            var mail = new MailMessage
            {
                From = new MailAddress(emailRequest.From),                
            };

            SetMailCollection(mail.To, emailRequest.To);
            SetMailCollection(mail.CC, emailRequest.Cc);
            SetMailCollection(mail.Bcc, emailRequest.Bcc);
            SetMailCollection(mail.ReplyToList, emailRequest.ReplyTo);

            mail.Subject = emailRequest.Subject;
            mail.IsBodyHtml = emailRequest.IsBodyHtml;
            mail.Body = emailRequest.Body;
            if (emailRequest.Attachments == null)
            {
                return mail;
            }
            foreach (var attachment in emailRequest.Attachments)
            {
                mail.Attachments.Add(new Attachment(attachment.Data, attachment.Filename));
            }

            return mail;
        }

        private static void SetMailCollection(MailAddressCollection collection, IEnumerable<string> addresses)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (addresses == null)
            {
                return;
            }

            foreach (var address in addresses.Select(a => new MailAddress(a)))
            {
                collection.Add(address);
            }
        }
    }
}
