using JWT_Token_Example.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace JWT_Token_Example.UtilityService;

public class Notification : INotification
{

    private readonly IConfiguration _config;
    public Notification(IConfiguration configuration)
    {
        _config = configuration;
    }

    public void SendEmailAsync(EmailModel emailModel)
    {
        var emailMessage = new MimeMessage();
        var from = _config["EmailSettings:From"];
        emailMessage.From.Add(new MailboxAddress("Order Placed", from));
        emailMessage.To.Add(new MailboxAddress(emailModel.To, emailModel.To));
        emailMessage.Subject = emailModel.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = string.Format(emailModel.Content)
        };
        
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                client.Send(emailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}