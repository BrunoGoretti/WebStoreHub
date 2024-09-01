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


    }
}
