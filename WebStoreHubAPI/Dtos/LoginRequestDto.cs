using System.ComponentModel.DataAnnotations;
using WebStoreHubAPI.Validation;

namespace WebStoreHubAPI.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [PasswordValidation]
        public string Password { get; set; }
    }
}
