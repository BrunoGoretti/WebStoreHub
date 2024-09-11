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
            _dbContext.DbProducts.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _dbContext.DbProducts.ToListAsync();
        }

        public async Task<ProductModel> GetProductByIdAsync(int productId)
        {
            return await _dbContext.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<ProductModel> UpdateProductAsync(int productId, string updateProductName, string updatedDescription, decimal updatePrice, int updatedStock, string updateImageUrl)
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
            existingProduct.ImageUrl = updateImageUrl;

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
