using Microsoft.AspNetCore.Mvc;

namespace WebStoreHubAPI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
