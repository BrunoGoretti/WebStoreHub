using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IBrandService
    {
        Task<BrandModel> CreateBrandAsync(BrandModel brand);
        Task<BrandModel> GetBrandByIdAsync(int brandId);
        Task<IEnumerable<BrandModel>> GetAllBrandsAsync();
        Task<BrandModel> UpdateBrandAsync(int brandId, string brandName);
        Task<bool> DeleteBrandAsync(int brandId);
    }
}
