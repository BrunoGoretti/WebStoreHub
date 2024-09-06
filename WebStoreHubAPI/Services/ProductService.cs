using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;

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

        public async Task<ProductModel> GetProductByProductIdAsync(int productId)
        {
            return await _dbContext.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<ProductModel> UpdateProductAsync(int productId, ProductModel updatedProduct)
        {
            var existingProduct = await _dbContext.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
            {
                return null; 
            }

            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock;
            existingProduct.ImageUrl = updatedProduct.ImageUrl;

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
