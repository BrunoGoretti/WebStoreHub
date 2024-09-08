using System.ComponentModel.DataAnnotations;
using WebStoreHubAPI.Validation;

namespace WebStoreHubAPI.Dtos
{
    public class UserRegistrationDto
    {
        [Required]
        [UsernameUniqueValidation]
        public string Username { get; set; }

        [Required]
        [PasswordValidation]
        public string PasswordHash { get; set; }

        [Required]
        [EmailUniqueValidation]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
