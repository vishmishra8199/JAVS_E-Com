using JWT_Token_Example.Context;
using JWT_Token_Example.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Token_Example.Repository;

public class UserRepository
{
    private readonly AppDbContext _appDbContext;
    
    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public Task<User> getUserData(string userName)
    {
        Task<User?> user =  _appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        return user;
    }

    public Task<User> getUserDataEamil(string email)
    {
        Task<User> user = _appDbContext.Users.FirstOrDefaultAsync(a => a.Email == email);
        return user;
    }

    public Task<User> getUserEmail(string email)
    {
        Task<User> user = _appDbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == email);
        return user;
    }

    public async Task updateUser(User user)
    {
        _appDbContext.Users.Update(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<bool> checkUser(string username)
    {
        return await _appDbContext.Users.AnyAsync(x => x.UserName == username);
    }
    
    public async Task<bool> checkEmail(string email)
    {
        return await _appDbContext.Users.AnyAsync(x => x.Email == email);
    }

    public async Task AddUser(User user)
    {
        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task updateEmailState(User user)
    {
        _appDbContext.Entry(user).State = EntityState.Modified;
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<User> getUserId(Guid id)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(a => a.Id == id);
    }

}