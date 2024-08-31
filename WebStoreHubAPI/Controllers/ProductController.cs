using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddProduct(ProductModel model)
        {
            var result = await _productService.CreateProductAsync(model);
            if (result == null)
            {
                return BadRequest("Product could not be created.");
            }
            return Ok(result);
        }
    }
}
