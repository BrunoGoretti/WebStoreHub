using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly AppDbContext _dbContext;

        public ProductTypeService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ProductTypeModel> CreateProductTypeAsync(ProductTypeModel productType)
        {
            _dbContext.DbProductTypes.Add(productType);
            await _dbContext.SaveChangesAsync();
            return productType;
        }

        public async Task<IEnumerable<ProductTypeModel>> GetAllProductsTypesAsync()
        {
            return await _dbContext.DbProductTypes.ToListAsync();
        }

        public async Task<ProductTypeModel> GetProductTypeByIdAsync(int productTypeId)
        {
            return await _dbContext.DbProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == productTypeId);
        }

        public async Task<ProductTypeModel> UpdateProductTypeAsync(int productTypeId, string ProductTypeName)
        {
            var existingType = await _dbContext.DbProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == productTypeId);

            if (existingType == null)
            {
                return null;
            }

            existingType.ProductTypeId = productTypeId;
            existingType.TypeName = ProductTypeName;

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