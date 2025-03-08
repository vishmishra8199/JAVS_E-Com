using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using JWT_Token_Example.Context;
using JWT_Token_Example.Helpers;
using JWT_Token_Example.Models;
using JWT_Token_Example.Models.DTO;
using JWT_Token_Example.UtilityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Token_Example.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly INotification _notification;
    private readonly IUserService _userService;
    private readonly IResetPassword _resetPassword;

    public UserController( IConfiguration configuration, IEmailService emailService, INotification notification, IUserService userService, IResetPassword resetPassword)
    {
        _configuration = configuration;
        _emailService = emailService;
        _notification = notification;
        _userService = userService;
        _resetPassword = resetPassword;
    }
    
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] User userObj)
    {
        try
        {
            //check if user object is null or not
            if (userObj == null)
            {
                return BadRequest(new
                {
                    Message = "userObj is null"
                });
            }
            
            // check is user is present in database
            var user = await _userService.getUserData(userObj);
            if (user == null)
            {
                return NotFound(new { Message = "User Not Found!" });
            }
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password ))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            user.Token =  _userService.CreateJwt(user);
            _userService.updateUser(user);
            return Ok(new
            {
                name = user.FirstName + user.LastName,
                role = user.Role,
                Guid = user.Id,
                Message = "Login Success!"
            });
        }
        catch
        {
            
        }

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User userObj)
    {
        try
        {
            if (userObj == null)
            {

                return BadRequest(new
                {
                    Message = "userobj is null"
                });
            }
            
            //check Username
        
            if (await _userService.CheckUserNameExistAsync(userObj.UserName))
            {
                return BadRequest(new
                {
                    Message= "Username Already Exist!"
                });
            }

            //check email
        
            if (await _userService.CheckEmailExistAsync(userObj.Email))
            {
                return BadRequest(new
                {
                    Message= "Email Already Exist!"
                });
            }
        
            //check password strength
            var pass = _userService.CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(pass))
            {
                return BadRequest(new
                {
                    Message = pass.ToString()
                });
            }
            
            userObj.Password = PasswordHasher.HashPassword(userObj.Password);
            userObj.Role = "User";
            userObj.Token = "";
            
            _userService.AddUser(userObj);
            return Ok(new
            {
                Message = "User Registered!"
            });
            
        }
        catch
        {
        }

        return Ok();
    }
    
    [HttpPost("getUser")]
    public async Task<IActionResult> UserDetails([FromBody] string id)
    {
        
        Guid newid;
        Guid.TryParse(id, out newid);
        User user = await _userService.Notification(newid);
        return Ok(new
        {
            firstname = user.FirstName,
            lastname = user.LastName,
            email = user.Email,
            phone = user.MobileNumber,
            username = user.UserName
        });
    }


    [HttpPost("send-reset-email/{email}")] 
    public async Task<IActionResult> SendEmail(string email)
    {
        var user = await _resetPassword.getUserData(email);
        if (user is null)
        {
            return NotFound(new
            {
                StatusCode = 404,
                Message = "Email Doesn't Exist"
            });
        }

        _resetPassword.sendEmail(user);
        
        return Ok(new
        {
            StatusCode = 200,
            Message = "Email Sent!"
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");
        var user = await _resetPassword.resetPassword(resetPasswordDto.Email);
        if (user is null)
        {
            return NotFound(new
            {
                StatusCode = 404,
                Message = "User Doesn't Exist"
            });
        }

        var tokenCode = user.ResetPasswordToken;
        DateTime emailTokenExpiry = user.ResetPasswordExpiry;
        if (tokenCode != resetPasswordDto.EmailToken || emailTokenExpiry < DateTime.Now)
        {
            return BadRequest(new
            {
                StatusCode = 400,
                Message = " Invalid Reset Link"
            });
        }

        user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
        _resetPassword.resetPasswordUpdate(user);
        return Ok(new
        {
            StatusCode = 200,
            Message = "Password Reset Successfully"
        });
    }

    [HttpPost("email-notification/{id}")]
    public async Task<IActionResult> SendNotification(string id)
    {
        Guid newid;
        Guid.TryParse(id, out newid);
        var user = await _userService.Notification(newid);
        
        return Ok(new
        {
            StatusCode = 200,
            Message = "Email Sent!"
        });
    }
    
}
