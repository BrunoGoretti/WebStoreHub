using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserRegistrationDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userModel = new UserModel
            {
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash,
                Email = userDto.Email,
                FullName = userDto.FullName,
                Role = UserRole.User
            };

            var result = await _userService.CreateUserAsync(userModel);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromQuery] string email, [FromQuery] string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Email and password are required.");
            }

            (string? token, string? username, UserRole role, string? fullName) = await _userService.LoginUserAsync(email, password);

            if (token != null)
            {
                return Ok(new { Token = token, Username = username, FullName = fullName, Role = role });
            }

            return Unauthorized("Invalid email or password");
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset(PasswordResetRequestDto request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var token = await _userService.GeneratePasswordResetTokenAsync(user);

            // Send token via email
            await _userService.SendPasswordResetEmailAsync(user.Email, token);

            return Ok("Password reset email sent.");
        }
    }
}
