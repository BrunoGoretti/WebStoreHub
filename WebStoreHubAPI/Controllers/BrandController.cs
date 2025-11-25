using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/[controller]")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost("addBrand")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddBrand([FromBody] AddBrandDto brand)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newBrand = new BrandModel
            {
                BrandName = brand.BrandName,
            };

            var result = await _brandService.CreateBrandAsync(newBrand);
            return Ok(result);
        }

        [HttpGet("getBrandById/{brandId}")]
        public async Task<IActionResult> GetBrandById(int brandId)
        {
            var brand = await _brandService.GetBrandByIdAsync(brandId);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(brand);
        }

        [HttpGet("getAllBrands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllBrandsAsync();
            return Ok(brands);
        }

        [HttpPut("updateBrand/{brandId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBrand(int brandId, string updatedbrandName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _brandService.UpdateBrandAsync(brandId, updatedbrandName);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpDelete("deleteBrand/{brandId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBrand(int brandId)
        {
            var success = await _brandService.DeleteBrandAsync(brandId);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
