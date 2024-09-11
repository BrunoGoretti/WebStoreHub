using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeService _productTypeService;

        public ProductTypeController(IProductTypeService productTypeService)
        {
            _productTypeService = productTypeService;
        }

        [HttpPost("addProductType")]
        public async Task<IActionResult> AddProductType(AddTypeDto type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productType = new ProductTypeModel
            {
                TypeName = type.TypeName
            };

            var result = await _productTypeService.CreateProductTypeAsync(productType);
            return Ok(result);
        }

        [HttpGet("getAllProductTypes")]
        public async Task<IActionResult> GetAllProductsTypes()
        {
            var productsTypes = await _productTypeService.GetAllProductsTypesAsync();
            return Ok(productsTypes);
        }

        [HttpGet("getProductTypesById")]
        public async Task<IActionResult> GetProductTypeById(int productTypeId)
        {
            var productType = await _productTypeService.GetProductTypeByIdAsync(productTypeId);
            if (productType == null)
            {
                return NotFound();
            }
            return Ok(productType);
        }

        [HttpPut("updateProductType")]
        public async Task<IActionResult> UpdateProductType(int productTypeId, string updatedProductType)
        {
            var productType = await _productTypeService.UpdateProductTypeAsync(productTypeId, updatedProductType);

            if (productType == null)
            {
                return NotFound();
            }

            return Ok(productType);
        }
    }
}
