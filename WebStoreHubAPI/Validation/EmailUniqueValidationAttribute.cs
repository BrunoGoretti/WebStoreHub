using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;

namespace WebStoreHubAPI.Validation
{
    public class EmailUniqueValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var email = value.ToString();

            var emailExists = _dbContext.DbUsers.AnyAsync(u => u.Email == email).Result;

            if (emailExists)
            {
                return new ValidationResult("This email is already registered.");
            }

            return ValidationResult.Success;
        }
    }
}