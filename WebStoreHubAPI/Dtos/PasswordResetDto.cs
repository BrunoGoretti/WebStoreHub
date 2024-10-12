using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Dtos
{
    public class PasswordResetDto
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
