using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Registration(UserModel user)
        {
            var result = await _userService.CreateUserAsync(user);
            return Ok(result);
        }
    }
}
