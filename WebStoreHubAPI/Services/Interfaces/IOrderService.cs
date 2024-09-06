using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrderAsync(int userId, List<CartItemModel> cartItems);
        Task<OrderModel> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
