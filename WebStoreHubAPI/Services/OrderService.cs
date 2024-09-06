using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderModel> CreateOrderAsync(int userId, List<CartItemModel> cartItems)
        {
            var totalAmount = cartItems.Sum(c => c.Product.Price * c.Quantity);

            var order = new OrderModel
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderItems = cartItems.Select(c => new OrderItemModel
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Product.Price
                }).ToList()
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            // Clear the cart after creating the order
            _dbContext.CartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();

            return order;
        }

        public async Task<OrderModel> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var order = await _dbContext.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return false;
            }

            order.Status = newStatus;
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
