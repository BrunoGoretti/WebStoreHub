using Microsoft.AspNetCore.Mvc;

namespace WebStoreHubAPI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
