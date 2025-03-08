using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace JWT_Token_Example.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int? MobileNumber { get; set; }
    public string? Token { get; set; }
    public string? Role { get; set; }
    public string? ResetPasswordToken { get; set; }
    public DateTime ResetPasswordExpiry { get; set; }
    
}