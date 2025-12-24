using Microsoft.EntityFrameworkCore;
using RenStore.Persistence;

namespace Tests.Common;

public class DatabaseFixture : IDisposable
{
    public static string ConnectionString = "Server=localhost;Port=5432;DataBase=UnitRenstoreTests; User Id=re;Password=postgres;Include Error Detail=True";
    
    public static ApplicationDbContext CreateReadyContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        var context = new ApplicationDbContext(options);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        SeedData(context);
        
        context.SaveChanges();
        context.ChangeTracker.Clear();
        
        return context;
    }

    private static void SeedData(ApplicationDbContext context)
    {
        context.Colors.AddRange(TestData.Colors);
        context.Countries.AddRange(TestData.Countries);
        context.Cities.AddRange(TestData.Cities);
        context.Categories.AddRange(TestData.Categories);
        context.SubCategories.AddRange(TestData.SubCategories);
        context.AspNetUsers.AddRange(TestData.Users);
        context.Sellers.AddRange(TestData.Sellers);
        context.Products.AddRange(TestData.Products);
        context.ProductVariants.AddRange(TestData.ProductVariants);
        context.ProductClothes.AddRange(TestData.ProductClothes);
        context.ProductAttributes.AddRange(TestData.ProductAttributes);
        context.ProductDetails.AddRange(TestData.ProductDetails);
        context.ProductClothSizes.AddRange(TestData.ProductClothSizes);
        context.PriceHistories.AddRange(TestData.PriceHistories);
        /*context.ProductImages.AddRange(TestData.ProductImages);*/
        /*context.SellerImages.AddRange(TestData.SellerImages);*/
    }
    
    public void Dispose() { }
}