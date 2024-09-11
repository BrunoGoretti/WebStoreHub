using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IProductTypeService
    {
        Task<ProductTypeModel> CreateProductTypeAsync(ProductTypeModel productType);
        Task<IEnumerable<ProductTypeModel>> GetAllProductsTypesAsync();
        Task<ProductTypeModel> GetProductTypeByIdAsync(int productTypeId);
        Task<ProductTypeModel> UpdateProductTypeAsync(int productTypeId, string ProductTypeName);
        Task<bool> RemoveProductTypeAsync(int productTypeId);
    }
}
