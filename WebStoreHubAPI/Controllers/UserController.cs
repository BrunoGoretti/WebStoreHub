using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration(UserRegistrationDto userDto)
        {
            var userModel = new UserModel
            {
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash,
                Email = userDto.Email,
                FullName = userDto.FullName,
            };

            var result = await _userService.CreateUserAsync(userModel);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginRequestDto loginRequest)
        {
            var user = await _userService.LoginUserAsync(loginRequest.Email, loginRequest.PasswordHash);
            if (user != null)
            {
                // If login is successful, you might return a token or user details
                return Ok(user);
            }

            // If login fails, return an unauthorized status
            return Unauthorized("Invalid username or password");
        }
    }
}
