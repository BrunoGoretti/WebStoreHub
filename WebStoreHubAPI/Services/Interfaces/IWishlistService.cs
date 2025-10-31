using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IWishlistService
    {
        Task AddToWishlist(int userId, int productId);
        Task RemoveFromWishlist(int userId, int productId);

        Task<List<WishlistItemModel>> GetWishlistItems(int userId);
    }
}
