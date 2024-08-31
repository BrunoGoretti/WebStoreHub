using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductModel> CreateProductAsync(ProductModel product);
    }
}
