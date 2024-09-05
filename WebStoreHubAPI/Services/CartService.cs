using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _dbContext;

        public CartService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddToCart(int userId, int productId, int quantity)
        {
            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItemModel
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _dbContext.CartItems.AddAsync(cartItem);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCartItem(int userId, int cartItemId, int quantity)
        {
            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.CartItemId == cartItemId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCart(int userId, int cartItemId)
        {
            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.CartItemId == cartItemId);

            if (cartItem != null)
            {
                _dbContext.CartItems.Remove(cartItem);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ClearCart(int userId)
        {
            var cartItems = await _dbContext.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _dbContext.CartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<CartItemModel>> GetCartItems(int userId)
        {
            return await _dbContext.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
    }
}
