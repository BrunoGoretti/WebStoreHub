using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> CreateUserAsync(UserModel user);

        Task<(string Token, string Username, UserRole Role, string FullName, int UserId)> LoginUserAsync(string email, string password);

        Task<bool> IsUsernameTakenAsync(string username);

        Task<string> GeneratePasswordResetTokenAsync(UserModel user);

        Task SendPasswordResetEmailAsync(string email, string token);

        Task<UserModel?> GetUserByEmailAsync(string email);

        Task<UserModel> GetUserByResetTokenAsync(string token);

        Task ResetPasswordAsync(UserModel user, string newPassword);
    }
}
