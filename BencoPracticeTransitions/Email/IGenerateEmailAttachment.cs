using System.Collections.Generic;
using BencoPracticeTransitions.Infrastructure.Email;

namespace BencoPracticeTransitions.Email
{
    public interface IGenerateEmailAttachment
    {
        EmailAttachment GenerateCsvEmailAttachment(List<KeyValuePair<string, string>> columns, string filename);
    }
}
