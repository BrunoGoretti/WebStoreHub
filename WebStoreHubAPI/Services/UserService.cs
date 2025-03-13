using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly string _jwtKey;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _configuration = configuration;
            _jwtKey = configuration["Jwt:Key"];
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _dbContext.DbUsers.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<(string Token, string Username, UserRole Role, string FullName, int UserId)> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.DbUsers.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                return (token, user.Username, user.Role, user.FullName, user.UserId);
            }

            return (null, null, default, null, 0);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dbContext.DbUsers.AnyAsync(u => u.Username == username);
        }

        public string GenerateJwtToken(UserModel user) 
        {
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
                throw new InvalidOperationException("JWT Key is not configured");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("UserId", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Directly return string
        }

        public async Task<string> GeneratePasswordResetTokenAsync(UserModel user)
        {
            var token = Guid.NewGuid().ToString();
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiration = DateTime.UtcNow.AddHours(1); // Token expires in 1 hour
            _dbContext.DbUsers.Update(user);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        public async Task SendPasswordResetEmailAsync(string email, string token)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPortString = _configuration["EmailSettings:SmtpPort"];
            var smtpEmail = _configuration["EmailSettings:EmailAddress"];
            var smtpPassword = _configuration["EmailSettings:EmailPassword"];

            // Check for missing configurations
            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPortString) ||
                string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
            {
                throw new ArgumentException("SMTP configuration is missing or invalid.");
            }

            // Parse SMTP Port
            if (!int.TryParse(smtpPortString, out var smtpPort))
            {
                throw new ArgumentException("SMTP port is invalid.");
            }

            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WebStoreHub", smtpEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Password Reset";

            var resetLink = $"https://yourfrontend.com/reset-password?token={token}";

            message.Body = new TextPart("plain")
            {
                Text = $"Please click the following link to reset your password: {resetLink}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpEmail, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task<UserModel?> GetUserByEmailAsync(string email)
        {
            return await _dbContext.DbUsers.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserModel> GetUserByResetTokenAsync(string token)
        {
            return await _dbContext.DbUsers.FirstOrDefaultAsync(u => u.PasswordResetToken == token);
        }

        public async Task ResetPasswordAsync(UserModel user, string newPassword)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordResetToken = null;  // Clear the token after use
            user.PasswordResetTokenExpiration = null;
            _dbContext.DbUsers.Update(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
