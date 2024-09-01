using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _userService;

        public UserService(AppDbContext context)
        {
            _userService = context;
        }
        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            _userService.DbUsers.Add(user);
            await _userService.SaveChangesAsync();
            return user;
        }
    }
}
