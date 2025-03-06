using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace WebStoreHubAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ProductModel> CreateProductAsync(ProductModel product)
        {
            var productType = await _dbContext.DbProductTypes
                .FirstOrDefaultAsync(pt => pt.ProductTypeId == product.ProductTypeId);

            var brand = await _dbContext.DbBrands
                .FirstOrDefaultAsync(b => b.BrandId == product.BrandId);

            if (productType == null)
            {
                throw new Exception("Invalid ProductTypeId");
            }

            if (brand == null)
            {
                throw new Exception("Invalid BrandId");
            }

            product.ProductType = productType;
            product.Brand = brand;

            _dbContext.DbProducts.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _dbContext.DbProducts
               .Include(p => p.ProductType)
               .Include(p => p.Brand)
               .Include(p => p.Images)
               .ToListAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            return await _dbContext.DbProducts
                .Include(p => p.ProductType)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<IEnumerable<ProductModel>> SearchProductsByNameAsync(string name)
        {
            return await _dbContext.DbProducts
                .Where(p => p.Name.ToLower().Contains(name.ToLower()) || p.Description.ToLower().Contains(name.ToLower()))
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<ProductModel> UpdateProductAsync(int productId, string updateProductName, string updatedDescription, decimal updatePrice, int updatedStock)
        {
            var existingProduct = await _dbContext.DbProducts.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
            {
                return null; 
            }

            existingProduct.Name = updateProductName;
            existingProduct.Description = updatedDescription;
            existingProduct.Price = updatePrice;
            existingProduct.Stock = updatedStock;

            await _dbContext.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _dbContext.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            _dbContext.DbProducts.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
