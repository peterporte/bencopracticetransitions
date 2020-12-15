namespace BencoPracticeTransitions.Infrastructure.Email
{
    public class BencoEmailMessageSettings
    {
        public string[] To { get; set; }

        public string[] Cc { get; set; }

        public string[] Bcc { get; set; }

        public string From { get; set; }

        public string[] ReplyTo { get; set; }
    }
}
