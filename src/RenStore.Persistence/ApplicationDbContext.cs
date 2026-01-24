using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Entities;
using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Entities;
using RenStore.Persistence.EntityTypeConfigurations;

namespace RenStore.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductVariantConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
        modelBuilder.ApplyConfiguration(new ShoppingCartItemConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new ProductQuestionConfiguration());
        modelBuilder.ApplyConfiguration(new ProductAnswerConfiguration());
        modelBuilder.ApplyConfiguration(new SellerImageConfiguration());
        modelBuilder.ApplyConfiguration(new UserImageConfiguration());
        modelBuilder.ApplyConfiguration(new AnswerComplainConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionComplainConfiguration());
        modelBuilder.ApplyConfiguration(new ProductVariantComplainConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewComplainConfiguration());
        modelBuilder.ApplyConfiguration(new SellerComplainConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
    
    public DbSet<ApplicationUser> AspNetUsers { get; set; }
    public DbSet<SellerEntity> Sellers { get; set; }
    public DbSet<Color> Colors { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductDetailEntity> ProductDetails { get; set; }
    public DbSet<ProductClothEntity> ProductClothes { get; set; }
    public DbSet<ProductClothSizeEntity> ProductClothSizes { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }
    public DbSet<ProductQuestionEntity> ProductQuestions { get; set; }
    public DbSet<ProductAnswerEntity> ProductAnswers { get; set; }
    public DbSet<ProductPriceHistoryEntity> PriceHistories { get; set; }
    public DbSet<ShoppingCartEntity> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItemEntity> ShoppingCartItems { get; set; }
    public DbSet<ReviewEntity> Reviews { get; set; }
    public DbSet<ProductImageEntity> ProductImages { get; set; }
    public DbSet<SellerImageEntity> SellerImages { get; set; }
    public DbSet<UserImageEntity> UserImages { get; set; }
    public DbSet<AnswerComplainEntity> AnswerComplains { get; set; }
    public DbSet<QuestionComplainEntity> QuestionComplains { get; set; }
    public DbSet<ProductVariantComplainEntity> ProductVariantComplains { get; set; }
    public DbSet<ReviewComplainEntity> ReviewComplains { get; set; }
    public DbSet<SellerComplainEntity> SellerComplains { get; set; }
    public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
    public DbSet<DeliveryTariff> DeliveryTariffs { get; set; }
    public DbSet<DeliveryTracking> DeliveryTrackingHistory { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderItemEntity> OrderItems { get; set; }
    public DbSet<PaymentEntity> Payments { get; set; }
    
}
