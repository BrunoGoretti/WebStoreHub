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
                return Ok(user);
            }

            return Unauthorized("Invalid username or password");
        }
    }
}
