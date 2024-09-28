using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
        private readonly string _jwtKey;

        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _dbContext = context;
            _jwtKey = configuration["Jwt:Key"];
        }

        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _dbContext.DbUsers.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<string> LoginUserAsync(string email, string password)
        {
            var user = await _dbContext.DbUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var token = await GenerateJwtToken(user);
                return token; 
            }

            return null;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dbContext.DbUsers.AnyAsync(u => u.Username == username);
        }

        public async Task<string> GenerateJwtToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
