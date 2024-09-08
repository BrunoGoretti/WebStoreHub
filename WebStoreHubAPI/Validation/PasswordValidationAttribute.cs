using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebStoreHubAPI.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        private const int MinimumPasswordLength = 6;

        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password.Length < MinimumPasswordLength)
            {
                return new ValidationResult($"Password must be at least {MinimumPasswordLength} characters long.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            return ValidationResult.Success;
        }

    }
}
