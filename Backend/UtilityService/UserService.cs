using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Config;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using JWT_Token_Example.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace JWT_Token_Example.UtilityService;

public class UserService : IUserService
{

    private readonly UserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly INotification _notification;
    
    public UserService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, INotification notification)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _notification = notification;
    }
    
    public async Task<User> getUserData(User userObj)
    {
        User user = await _userRepository.getUserData(userObj.UserName);

        return user;
        
    }
    
    public async Task<bool> CheckUserNameExistAsync(string username)
    {
        return await _userRepository.checkUser(username);
    }

    public async Task<bool> CheckEmailExistAsync(string email)
    {
        return await _userRepository.checkEmail(email);
    }

    public string CheckPasswordStrength(string password)
    {
        StringBuilder sb = new StringBuilder();
        if (password.Length < 8)
        {
            sb.Append("Minimum password length should be 8" + Environment.NewLine);
        }

        if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                                               && Regex.IsMatch(password, "[0-9]")))
        {
            sb.Append("Password Should be Alphanumeric" + Environment.NewLine);
        }

        if (!(Regex.IsMatch(password, "[<,>@!#$%^&*()_+\\[\\]{}?:;|'./~`=]")))
        {
            sb.Append("Password should contain special char" + Environment.NewLine);
        }

        return sb.ToString();
    }

    public void AddUser(User user)
    {
        _userRepository.AddUser(user);
    }

    public void updateUser(User user)
    {
        _userRepository.updateUser(user);
    }
    
    public string CreateJwt(User user)
    {
        //header.payload.signature
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("TPDNmk5ADponVEiQc5tmRkHhOiAFmkAr");
        var identity = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim("Guid", user.Id.ToString())
        });
        
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = credentials
        };
        
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var encrypterToken = jwtTokenHandler.WriteToken(token);
        var httpContext = _httpContextAccessor.HttpContext;
        
        httpContext.Response.Cookies.Append("token", encrypterToken,
            new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            });
        return encrypterToken;
    }

    public Task<User> Notification(Guid id)
    {
        return _userRepository.getUserId(id);
    }

    public void sendNoti(User user)
    {
        string orderNumber = "123";
        var address = "Swimlane Hyderabad";
        string email = user.Email;
        
        string from = _configuration["EmailSettings:From"];
        var emailModel = new EmailModel(email, "Order Confirmation", UserNotificationBody.UserNotificationMail(orderNumber,address));
        _notification.SendEmailAsync(emailModel);
        _userRepository.updateEmailState(user);
    }
}