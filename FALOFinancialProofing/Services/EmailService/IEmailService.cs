using FALOFinancialProofing.DTOs;

namespace FALOFinancialProofing.Services.EmailService
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
