using JWT_Token_Example.Context;
using JWT_Token_Example.Models.DTO;
using Microsoft.AspNetCore.Mvc;
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

namespace JWT_Token_Example.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorController : ControllerBase
{
    private readonly AppDbContext _authContext;
    
    public VendorController(AppDbContext appDbContext)
    {
        _authContext = appDbContext;
    }

    [HttpPost("vendorRegister")]
    public async Task<IActionResult> VendorRegister([FromBody] VendorDto vendorObj)
    {
        //check if user object is null or not
        if (vendorObj == null)
        {
            return BadRequest(new
            {
                Message = "userObj is null"
            });
        }
        
        //check Username
        
        if (await CheckUserNameExistAsync(vendorObj.UserName))
        {
            return BadRequest(new
            {
                Message= "Username Already Exist!"
            });
        }

        //check email
        
        if (await CheckEmailExistAsync(vendorObj.Email))
        {
            return BadRequest(new
            {
                Message= "Email Already Exist!"
            });
        }
        
        //check password strength
        var pass = CheckPasswordStrength(vendorObj.Password);
        if (!string.IsNullOrEmpty(pass))
        {
            return BadRequest(new
            {
                Message = pass.ToString()
            });
        }

        vendorObj.Password = PasswordHasher.HashPassword(vendorObj.Password);
        vendorObj.Role = "Vendor";
        vendorObj.Token = "";

        Vendor vendor = new Vendor();
        vendor.vendorId = vendorObj.Id;
        vendor.AccountNumber = vendorObj.AccountNumber;
        vendor.GST = vendorObj.GST;
        vendor.PAN = vendorObj.PAN;

        User userobj = new User();
        userobj.Id = vendorObj.Id;
        userobj.FirstName = vendorObj.FirstName;
        userobj.LastName = vendorObj.LastName;
        userobj.UserName = vendorObj.UserName;
        userobj.Email = vendorObj.Email;
        userobj.Password = vendorObj.Password;
        userobj.Role = vendorObj.Role;
        userobj.MobileNumber = vendorObj.MobileNumber;
        userobj.ResetPasswordExpiry = vendorObj.ResetPasswordExpiry;
        userobj.ResetPasswordToken = vendorObj.ResetPasswordToken;
        
        await _authContext.Vendors.AddAsync(vendor);
        await _authContext.Users.AddAsync(userobj);
        await _authContext.SaveChangesAsync();
        
        return Ok(new
        {
            Message = "User Registered!"
        });
        
    }
    
    private async Task<bool> CheckUserNameExistAsync(string username)
    {
        return await _authContext.Users.AnyAsync(x => x.UserName == username);
    }

    private async Task<bool> CheckEmailExistAsync(string email)
    {
        return await _authContext.Users.AnyAsync(x => x.Email == email);
    }

    private string CheckPasswordStrength(string password)
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

}