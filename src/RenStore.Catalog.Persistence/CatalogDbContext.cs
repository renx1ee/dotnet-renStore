using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantAttributes;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> Variants { get; set; }
    public DbSet<VariantAttribute> Attributes { get; set; }
    /*public DbSet<ProductDetail> Details { get; set; }*/
    public DbSet<VariantSize> Cloths { get; set; }
    public DbSet<ProductImage> Images { get; set; }
}