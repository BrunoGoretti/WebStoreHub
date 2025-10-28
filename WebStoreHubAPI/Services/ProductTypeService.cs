using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AppDbContext _dbContext;
        private readonly IImgbbService _imgbbService;

        public ProductTypeService(AppDbContext context, IImgbbService imgbbService)
        {
            _dbContext = context;
            _imgbbService = imgbbService;
        }

        public async Task<ProductTypeModel> CreateProductTypeAsync(ProductTypeModel productType, IFormFile imageFile)
        {
            if (imageFile != null)
            {
                var imageUrl = await _imgbbService.UploadToImgbbAsync(imageFile);
                if (imageUrl == null) throw new Exception("Failed to upload image to Imgbb");
                productType.ImageUrl = imageUrl;
            }

            _dbContext.DbProductTypes.Add(productType);
            await _dbContext.SaveChangesAsync();
            return productType;
        }

        public async Task<IEnumerable<ProductTypeModel>> GetAllProductsTypesAsync()
        {
            return await _dbContext.DbProductTypes
                .OrderBy(pt => pt.DisplayOrder) 
                .ToListAsync();
        }

        public async Task<ProductTypeModel> GetProductTypeByIdAsync(int productTypeId)
        {
            return await _dbContext.DbProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == productTypeId);
        }

        public async Task<ProductTypeModel> UpdateProductTypeAsync(int productTypeId, string productTypeName, IFormFile imageFile)
        {
            var existingType = await _dbContext.DbProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == productTypeId);

            if (existingType == null)
            {
                return null;
            }

            existingType.TypeName = productTypeName;

            if (imageFile != null)
            {
                var imageUrl = await _imgbbService.UploadToImgbbAsync(imageFile);
                if (imageUrl == null) throw new Exception("Failed to upload image to Imgbb");
                existingType.ImageUrl = imageUrl;
            }

            await _dbContext.SaveChangesAsync();
            return existingType;
        }

        public async Task<bool> RemoveProductTypeAsync(int productTypeId)
        {
            var productType = await _dbContext.DbProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == productTypeId);

            if (productType == null) 
            {
                return false;
            }

            _dbContext.DbProductTypes.Remove(productType);
            await _dbContext.SaveChangesAsync();
            return true; 
        }
    }
}