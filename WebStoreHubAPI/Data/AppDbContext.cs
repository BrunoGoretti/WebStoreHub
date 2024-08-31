using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration can go here if needed
            // e.g., relationships, default values, indexes, etc.
        }
    }
}
