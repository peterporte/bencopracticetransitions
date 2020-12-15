using System.IO;

namespace BencoPracticeTransitions.Infrastructure.Email
{
    public class EmailAttachment
    {
        public Stream Data { get; set; }
        public string Filename { get; set; }
    }
}
