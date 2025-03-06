using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IDiscountService _discountService;
        public ProductController(IProductService productService, IDiscountService discountService) 
        {
            _productService = productService;
            _discountService = discountService;
        }

        [HttpPost("addProduct")]
        //[Authorize(Roles = "Admin")]
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
                ProductTypeId = dto.ProductTypeId,
                BrandId = dto.BrandId
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

        [HttpGet("getProductById/{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("searchProductsByName")]
        public async Task<IActionResult> SearchProductsByName(string name)
        {
            var products = await _productService.SearchProductsByNameAsync(name);
            return Ok(products);
        }

        [HttpPut("updateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int productId, string updateProductName, string updatedDescription, decimal updatePrice, int updatedStock)
        {
            var product = await _productService.UpdateProductAsync(productId, updateProductName, updatedDescription, updatePrice, updatedStock);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("deleteProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var success = await _productService.DeleteProductAsync(productId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent(); 
        }

        [HttpPost("uploadDiscounts")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadDiscounts(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (_discountService == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Discount service is not available.");
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    // Import discounts from the Excel file
                    var discounts = await _discountService.ImportDiscountsFromExcelAsync(stream);

                    // Apply discounts to the products
                    await _discountService.ApplyDiscountsAsync(discounts);
                }

                return Ok("Discounts applied successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while processing the file: {ex.Message}");
            }
        }
    }
}
