using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task AddToCart(int userId, int productId, int quantity);
        Task UpdateCartItem(int userId, int cartItemId, int quantity);
        Task RemoveFromCart(int userId, int cartItemId);
        Task ClearCart(int userId);
        Task<List<CartItemModel>> GetCartItems(int userId);
    }
}
