using Microsoft.EntityFrameworkCore;
using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options) { }

        public DbSet<UserModel> DbUsers { get; set; }
        public DbSet<ProductModel> DbProducts { get; set; }
        public DbSet<ProductTypeModel> DbProductTypes { get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderItemModel> OrderItems { get; set; }
        public DbSet<BrandModel> DbBrands { get; set; }
        public DbSet<DiscountModel> Discounts { get; set; }


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

            modelBuilder.Entity<DiscountModel>()
              .HasKey(d => d.DiscountId);

            modelBuilder.Entity<DiscountModel>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
