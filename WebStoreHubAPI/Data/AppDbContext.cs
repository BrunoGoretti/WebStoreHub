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
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        //public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItemModel>()
                .HasKey(c => new { c.UserId, c.ProductId });

            modelBuilder.Entity<CartItemModel>()
                .Property(c => c.CartItemId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderItemModel>()
            .HasKey(oi => oi.OrderItemId);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItemModel>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
        }
    }
}
