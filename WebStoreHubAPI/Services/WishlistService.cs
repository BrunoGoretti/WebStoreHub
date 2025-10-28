using Org.BouncyCastle.Bcpg;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Services.Interfaces;
using WebStoreHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace WebStoreHubAPI.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly AppDbContext _dbContext;

        public WishlistService(AppDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddToWishlist(int userId, int productId)
        {
            var addToWishlist = await _dbContext.DbWishlist.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);

            addToWishlist = new WishlistItemModel
            {
                UserId = userId,
                ProductId = productId,
            };

            _dbContext.DbWishlist.Add(addToWishlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveFromWishlist(int userId, int productId)
        {
            var removeFromWishlist = await _dbContext.DbWishlist.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);


            _dbContext.DbWishlist.Remove(removeFromWishlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<WishlistItemModel>> GetWishlistItems(int userId)
        {
            return await _dbContext.DbWishlist
               .Where(c => c.UserId == userId).ToListAsync();
        }
    }
}
