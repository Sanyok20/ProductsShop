using Microsoft.EntityFrameworkCore;
using Products.Models;

namespace Products
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) 
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);

                e.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired();

                e.Property(c => c.Description)
                .HasMaxLength(100);

                e.Property(c => c.Image)
                .HasMaxLength(100);
            });

            builder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);

                e.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

                e.Property(p => p.Price)
                .HasDefaultValue(0);

                e.Property(p => p.Image)
                .HasMaxLength(100);

                e.Property(p => p.Description)
                .HasMaxLength(100);
            });

            builder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();
        }
    }
}
