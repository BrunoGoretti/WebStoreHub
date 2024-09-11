using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _dbContext;
        private readonly IProductService _productService;


        public OrderService(AppDbContext dbContext, IProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        public async Task<OrderModel> CreateOrderAsync(int userId, List<CartItemModel> cartItems)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
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

                    var paymentSuccess = SimulatePayment(totalAmount);
                    if (!paymentSuccess)
                    {
                        throw new InvalidOperationException("Payment failed.");
                    }

                    foreach (var cartItem in cartItems)
                    {
                        var product = await _productService.GetProductByIdAsync(cartItem.ProductId);
                        if (product.Stock >= cartItem.Quantity)
                        {
                            product.Stock -= cartItem.Quantity;
                            await _productService.UpdateProductAsync(product.ProductId, product.Name, product.Description, product.Price, product.Stock, product.ImageUrl);
                        }
                        else
                        {
                            throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                        }

                    }

                    await _dbContext.Orders.AddAsync(order);
                    await _dbContext.SaveChangesAsync();

                    _dbContext.CartItems.RemoveRange(cartItems);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    order.Status = "Confirmed";
                    await _dbContext.SaveChangesAsync();

                    return order;
                }
                catch (Exception ex)
                {
                    // If any exception occurs, rollback the transaction
                    await transaction.RollbackAsync();
                    throw new InvalidOperationException($"Order creation failed: {ex.Message}");
                }
            }
        }

        // Simulate payment (placeholder method)
        private bool SimulatePayment(decimal totalAmount)
        {
            // Simulate a payment success
            return true;
        }

        public async Task<OrderModel> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                   .Include(o => o.OrderItems)
                       .ThenInclude(oi => oi.Product)
                   .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<IEnumerable<OrderModel>> GetOrdersByUserIdAsync(int userId)
        {
            return await _dbContext.Orders
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
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
