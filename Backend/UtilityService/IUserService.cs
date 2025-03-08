using JWT_Token_Example.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Token_Example.UtilityService;

public interface IUserService
{
    public Task<User> getUserData(User userName);
    public Task<bool> CheckUserNameExistAsync(string username);

    public Task<bool> CheckEmailExistAsync(string email);

    public string CheckPasswordStrength(string password);

    public void AddUser(User user);
    public void updateUser(User user);

    public string CreateJwt(User user);

    public Task<User> Notification(Guid id);

}