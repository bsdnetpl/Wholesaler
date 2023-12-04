using Microsoft.EntityFrameworkCore;
using Wholesaler.Models;

namespace Wholesaler.DB
{
    public class MssqlConnect : DbContext
    {
        public MssqlConnect(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Products> ProductsDB { get; set; }
        public DbSet<Inventory> InventoriesDB { get; set; }
        public DbSet<Prices> PricesDB { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add unique index on SKU property of Products entity
            modelBuilder.Entity<Products>().HasIndex(e => e.SKU).IsUnique();
            // Add unique index on SKU property of Inventory entity
            modelBuilder.Entity<Inventory>().HasIndex(e => e.SKU).IsUnique();
            // Add composite primary key on Product_id property of Inventory entity
            modelBuilder.Entity<Inventory>().HasKey(e => e.Product_id);
            // Add unique index on SKU property of Prices entity
            modelBuilder.Entity<Prices>().HasIndex(e => e.SKU).IsUnique();
        }
    }
}
