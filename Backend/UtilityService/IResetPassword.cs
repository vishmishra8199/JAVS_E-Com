using JWT_Token_Example.Models;

namespace JWT_Token_Example.UtilityService;

public interface IResetPassword
{
    public Task<User> getUserData(string email);

    public string sendEmail(User user);

    public Task<User> resetPassword(string email);
    public void resetPasswordUpdate(User user);
}