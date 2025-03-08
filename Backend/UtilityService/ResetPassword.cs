using System.Security.Cryptography;
using Config;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using JWT_Token_Example.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace JWT_Token_Example.UtilityService;

public class ResetPassword : IResetPassword
{
    private readonly UserRepository _userRepository;
    private readonly Configuration _configuration;
    private readonly IEmailService _emailService;
    
    public ResetPassword(UserRepository userRepository, Configuration configuration, IEmailService emailService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _emailService = emailService;
    }
    public  Task<User> getUserData(string email)
    {
        return  _userRepository.getUserDataEamil(email);
    }

    public string sendEmail(User user)
    {
        string email = user.Email;
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var emailToken = Convert.ToBase64String(tokenBytes);
        user.ResetPasswordToken = emailToken;
        user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
        // string from = _configuration["EmailSettings:From"];
        var emailModel = new EmailModel(email, "Reset Password!", EmailBody.EmailStringBody(email, emailToken));
        _emailService.SendEmail(emailModel);
        _userRepository.updateEmailState(user);
        return "Ok";
    }

    public Task<User> resetPassword(string email)
    {
        return _userRepository.getUserEmail(email);
    }

    public void resetPasswordUpdate(User user)
    {
        _userRepository.updateEmailState(user);
    }



}