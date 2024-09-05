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
            _dbContext.DbUsers.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<UserModel> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.DbUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && user.PasswordHash == password)
            {
                return user;
            }

            return null;
        }
    }
}
