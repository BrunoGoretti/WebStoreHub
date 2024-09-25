using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> AddProduct(ProductCreationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = new ProductModel
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                ProductTypeId = dto.ProductTypeId 
            };

            var result = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { productId = result.ProductId }, result);
        }

        [HttpGet("getAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("getProductById")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> UpdateProduct(int productId, string updateProductName, string updatedDescription, decimal updatePrice, int updatedStock, string updateImageUrl)
        {
            var product = await _productService.UpdateProductAsync(productId, updateProductName, updatedDescription, updatePrice, updatedStock, updateImageUrl);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var success = await _productService.DeleteProductAsync(productId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent(); 
        }
    }
}
