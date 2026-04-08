/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductRepository;

public class AddRangeAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: CatalogRepositoryTestsBase
                .BuildConnectionString(Guid.NewGuid()))
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }

    [Fact]
    public async Task Should_Saved_Products_To_Postgres()
    {
        // Arrange
        var sellerId1 = 23242;
        var subCategoryId1 = Guid.NewGuid();
        var now1 = DateTimeOffset.UtcNow;
        
        var sellerId2 = 535353;
        var subCategoryId2 = Guid.NewGuid();
        var now2 = DateTimeOffset.UtcNow.AddHours(1);
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductProjection(
                _context, 
                eventStoreMock.Object);

        var products = new List<Product>()
        {
            Product.Create(
                sellerId: sellerId1,
                subCategoryId: subCategoryId1,
                now: now1),
            
            Product.Create(
                sellerId: sellerId2,
                subCategoryId: subCategoryId2,
                now: now2)
        };

        // Act
        await repository.AddRangeAsync(products, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var product1 = await _context.Products.
            FirstOrDefaultAsync(x => x.Id == products[0].Id);
        
        var product2 = await _context.Products.
            FirstOrDefaultAsync(x => x.Id == products[1].Id);
        
        Assert.Equal(2, await _context.Products.CountAsync());
        
        Assert.NotNull(product1);
        Assert.Equal(sellerId1, product1.SellerId);
        Assert.Equal(subCategoryId1, product1.SubCategoryId);
        Assert.Equal(now1, product1.CreatedAt);
        
        Assert.NotNull(product2);
        Assert.Equal(sellerId2, product2.SellerId);
        Assert.Equal(subCategoryId2, product2.SubCategoryId);
        Assert.Equal(now2, product2.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_When_Products_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductProjection(_context, eventStoreMock.Object);

        List<Product> products = null;
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await repository.AddRangeAsync(products, CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Not_Save_When_Categories_List_Count_0()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductProjection(_context, eventStoreMock.Object);

        var products = new List<Product>();
        
        // Act
        await repository.AddRangeAsync(products, CancellationToken.None);
        
        // Assert
        Assert.Equal(0, await _context.Products.CountAsync());
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/