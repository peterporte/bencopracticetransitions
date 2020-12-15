namespace BencoPracticeTransitions.Infrastructure.Email
{
    public interface ISendEmail
    {
        void Send(EmailRequest emailRequest);
    }
}
