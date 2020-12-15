using System.Collections.Generic;

namespace BencoPracticeTransitions.Infrastructure.Email
{
    public class EmailRequest
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string[] To { get; set; }
        public string From { get; set; }
        public string[] Cc { get; set; }
        public string[] Bcc { get; set; }
        public string[] ReplyTo { get; set; }
        public bool IsBodyHtml { get; set; }
        public IEnumerable<EmailAttachment> Attachments { get; set; }
    }
}
