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

        public DateTime CreatedAt { get; set; }
    }
}
