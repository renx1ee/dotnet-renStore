using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Catalog.Persistence.EntityTypeConfigurations;
using RenStore.Catalog.Persistence.EventStore;

namespace RenStore.Catalog.Persistence;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventStoreConfiguration());
        
        modelBuilder.ApplyConfiguration(new VariantAttributeConfiguration());
        modelBuilder.ApplyConfiguration(new VariantSizeConfiguration());        
        modelBuilder.ApplyConfiguration(new VariantPriceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new VariantDetailConfiguration());
        modelBuilder.ApplyConfiguration(new VariantImageConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<EventEntity> Events { get; set; }

    /*public DbSet<Color> Colors { get; set; }*/
    public DbSet<CategoryReadModel> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<ProductReadModel> Products { get; set; }
    public DbSet<ProductVariantReadModel> Variants { get; set; }
    public DbSet<VariantAttributeReadModel> Attributes { get; set; }
    public DbSet<VariantDetailReadModel> Details { get; set; }
    public DbSet<VariantSizeReadModel> Sizes { get; set; }
    public DbSet<PriceHistoryReadModel> Prices { get; set; }
    public DbSet<VariantImageReadModel> Images { get; set; }
}