using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace WebStoreHubAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _products;

        public async Task<ProductModel> CreateProductAsync(ProductModel product)
        {
            _products.Products.Add(product);
            await _products.SaveChangesAsync();
            return product;
        }
        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _products.Products.ToListAsync();
        }

        public async Task<ProductModel> GetProductByProductIdAsync(int productId)
        {
            return await _products.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
        public async Task<ProductModel> UpdateProductAsync(int productId, ProductModel updatedProduct)
        {
            var existingProduct = await _products.Products
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
            var product = await _products.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                return false;
            }

            _products.Products.Remove(product);
            await _products.SaveChangesAsync();
            return true;
        }
    }
}
