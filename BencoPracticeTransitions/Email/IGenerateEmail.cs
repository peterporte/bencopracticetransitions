using BencoPracticeTransitions.Infrastructure.Email;

namespace BencoPracticeTransitions.Email
{
    public interface IGenerateEmail
    {
        EmailRequest GenerateEmail(object model);
        bool CanGenerateEmailFromModel(object model);
    }
}
