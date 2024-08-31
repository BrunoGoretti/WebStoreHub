using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Data;

namespace WebStoreHubAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _products;

        public async Task<ProductModel> CreateProductAsync(ProductModel product)
        {
            var newProduct = new ProductModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl
            };

            _products.Add(newProduct);
            await _products.SaveChangesAsync();
            return newProduct;
        }
    }
}
