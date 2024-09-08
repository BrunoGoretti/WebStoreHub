using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;

namespace WebStoreHubAPI.Validation
{
    public class UsernameUniqueValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var _dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext));
            var username = value.ToString();

            var userExists = _dbContext.DbUsers.AnyAsync(u => u.Username == username).Result;

            if (userExists)
            {
                return new ValidationResult("This username is already taken.");
            }

            return ValidationResult.Success;
        }
    }
}