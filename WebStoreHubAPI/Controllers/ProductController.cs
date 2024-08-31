using Microsoft.AspNetCore.Mvc;

namespace WebStoreHubAPI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
