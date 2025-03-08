using JWT_Token_Example.Models;

namespace JWT_Token_Example.UtilityService;

public interface INotification
{ 
    void SendEmailAsync(EmailModel emailModel);
}