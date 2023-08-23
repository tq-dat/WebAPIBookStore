using Microsoft.EntityFrameworkCore;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }


        public DbSet<CartItem> CartItems { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                    .HasKey(pc => new { pc.ProductId, pc.CategoryId });
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(p => p.Product)
                    .WithMany(pc => pc.ProductCategories)
                    .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<ProductCategory>()
                    .HasOne(c => c.Category)
                    .WithMany(pc => pc.ProductCategories)
                    .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<CartItem>()
                    .HasKey(ci => new { ci.Id });
            modelBuilder.Entity<CartItem>()
                    .HasOne(p => p.Product)
                    .WithMany(ci => ci.CartItems)
                    .HasForeignKey(p => p.ProductId);
            modelBuilder.Entity<CartItem>()
                    .HasOne(o => o.Order)
                    .WithMany(ci => ci.CartItems)
                    .HasForeignKey(o => o.OrderId);
            modelBuilder.Entity<CartItem>()
                    .HasOne(u => u.User)
                    .WithMany(ci => ci.CartItems)
                    .HasForeignKey(u => u.UserId);
        }
    }
}


