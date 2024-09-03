using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace WebStoreHubAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _products;

        public ProductService(AppDbContext context)
        {
            _products = context;
        }

        public async Task<ProductModel> CreateProductAsync(ProductModel product)
        {
            _products.DbProducts.Add(product);
            await _products.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _products.DbProducts.ToListAsync();
        }

        public async Task<ProductModel> GetProductByProductIdAsync(int productId)
        {
            return await _products.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<ProductModel> UpdateProductAsync(int productId, ProductModel updatedProduct)
        {
            var existingProduct = await _products.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct == null)
            {
                return null; // Or throw an exception if you prefer
            }

            // Update properties
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Stock = updatedProduct.Stock;
            existingProduct.ImageUrl = updatedProduct.ImageUrl;

            await _products.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _products.DbProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            _products.DbProducts.Remove(product);
            await _products.SaveChangesAsync();
            return true;
        }
    }
}
