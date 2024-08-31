using Microsoft.AspNetCore.Mvc;

namespace WebStoreHubAPI.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
