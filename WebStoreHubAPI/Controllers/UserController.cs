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
        public async Task<IActionResult> LoginUser(LoginRequestDto loginRequest)
        {
            var token = await _userService.LoginUserAsync(loginRequest.Email, loginRequest.PasswordHash);
            if (token != null)
            {
                return Ok(new { Token = token }); // Return the token in the response
            }

            return Unauthorized("Invalid username or password"); // Handle the case where token is null
        }
    }
}
