using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Persistence.EntityTypeConfigurations;
using RenStore.Catalog.Persistence.EventStore;

namespace RenStore.Catalog.Persistence;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventStoreConfiguration());
        
        modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
        modelBuilder.ApplyConfiguration(new VariantSizeConfiguration());        
        modelBuilder.ApplyConfiguration(new ProductPriceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<EventEntity> Events { get; set; }

    /*public DbSet<Color> Colors { get; set; }*/
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> Variants { get; set; }
    public DbSet<VariantAttribute> Attributes { get; set; }
    public DbSet<VariantDetail> Details { get; set; }
    public DbSet<VariantSize> Sizes { get; set; }
    public DbSet<PriceHistory> Prices { get; set; }
    public DbSet<VariantImage> Images { get; set; }
}