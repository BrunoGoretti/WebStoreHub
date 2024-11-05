using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public UserRole Role { get; set; }

        public string? PasswordResetToken { get; set; }

        public DateTime? PasswordResetTokenExpiration { get; set; }

        public ICollection<OrderModel> Orders { get; set; }
    }

    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
}
