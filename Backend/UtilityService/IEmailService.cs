using JWT_Token_Example.Models;

namespace JWT_Token_Example.UtilityService;

public interface IEmailService
{
    void SendEmail(EmailModel emailModel);
}