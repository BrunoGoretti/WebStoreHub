using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
                : base(options){}

        public DbSet<UserModel> DbUsers { get; set; }
        public DbSet<ProductModel> DbProducts { get; set; }
/*        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }*/

    }
}
