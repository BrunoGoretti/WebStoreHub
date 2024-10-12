using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Dtos
{
    public class PasswordResetRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
