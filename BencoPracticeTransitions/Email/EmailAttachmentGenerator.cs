using System;
using BencoPracticeTransitions.Infrastructure.Email;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BencoPracticeTransitions.Email
{
    public class EmailAttachmentGenerator :IGenerateEmailAttachment
    {
        public EmailAttachment GenerateCsvEmailAttachment(List<KeyValuePair<string, string>> columns, string filename)
        {
            if (columns == null)
            {
                throw new ArgumentNullException(nameof(columns));
            }

            if (string.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentException("Value not specified.", nameof(filename));
            }

            var csv =
                $"\"{string.Join("\",\"", columns.Select(k => k.Key))}\"\n\"{string.Join("\",\"", columns.Select(v => v.Value?.Replace("\"", "\"\"")))}\"\n";
            var bytarray = Encoding.UTF8.GetBytes(csv);
            var bytarrayWithBom = Encoding.UTF8.GetPreamble().Concat(bytarray).ToArray();
            var stream = new MemoryStream(bytarrayWithBom);

            return new EmailAttachment { Data = stream, Filename = filename };
        }
    }
}
