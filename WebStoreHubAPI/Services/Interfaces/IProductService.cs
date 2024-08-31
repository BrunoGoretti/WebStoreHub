using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductModel> CreateProductAsync(ProductModel product);
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task<ProductModel> GetProductByProductIdAsync(int productId);
        Task<ProductModel> UpdateProductAsync(int productId, ProductModel updatedProduct);
        Task<bool> DeleteProductAsync(int productId);
    }
}
