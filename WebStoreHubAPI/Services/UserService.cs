using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _dbContext.DbUsers.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.DbUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dbContext.DbUsers.AnyAsync(u => u.Username == username);
        }
    }
}
