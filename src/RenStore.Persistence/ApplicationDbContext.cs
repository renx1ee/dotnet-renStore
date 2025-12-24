using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Persistence.EntityTypeConfigurations;

namespace RenStore.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ColorConfiguration());
        modelBuilder.ApplyConfiguration(new SellerConfiguration());
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SubCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
        modelBuilder.ApplyConfiguration(new ProductDetailConfiguration());
        modelBuilder.ApplyConfiguration(new ProductClothConfiguration());
        modelBuilder.ApplyConfiguration(new ProductClothSizeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductAttributeConfiguration());
        modelBuilder.ApplyConfiguration(new ProductPriceHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartItemConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new ProductQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new ProductAnswerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductImageConfiguration());
        modelBuilder.ApplyConfiguration(new SellerImageConfiguration());
        modelBuilder.ApplyConfiguration(new UserImageConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<SellerEntity> Sellers { get; set; }
    public DbSet<ColorEntity> Colors { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CountryEntity> Countries { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<SubCategoryEntity> SubCategories { get; set; }
    public DbSet<CityEntity> Cities { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductVariantEntity> ProductVariants { get; set; }
    public DbSet<ProductDetailEntity> ProductDetails { get; set; }
    public DbSet<ProductClothEntity> ProductClothes { get; set; }
    public DbSet<ProductClothSizeEntity> ProductClothSizes { get; set; }
    public DbSet<ProductAttributeEntity> ProductAttributes { get; set; }
    public DbSet<ProductQuestionEntity> ProductQuestions { get; set; }
    public DbSet<ProductAnswerEntity> ProductAnswers { get; set; }
    public DbSet<ProductPriceHistoryEntity> PriceHistories { get; set; }
    public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItemEntity> ShoppingCartItems { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<SellerImageEntity> SellerImages { get; set; }
    public DbSet<UserImageEntity> UserImages { get; set; }
}
