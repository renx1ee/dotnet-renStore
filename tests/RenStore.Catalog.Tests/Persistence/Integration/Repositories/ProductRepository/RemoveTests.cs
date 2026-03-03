/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductRepository;

public class RemoveTests : IAsyncLifetime
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
    public async Task Should_Removed_Product_From_Postgres()
    {
        // Arrange
        var sellerId = 23242;
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductProjection(
                _context, 
                eventStoreMock.Object);

        var product = Product.Create(
            sellerId: sellerId,
            subCategoryId: subCategoryId,
            now: now);
        
        var productId = await repository
            .AddAsync(product, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        var savedProduct = await _context.Products
            .FirstOrDefaultAsync(x => 
                x.Id == product.Id);

        Assert.NotNull(savedProduct);
        Assert.Equal(sellerId, savedProduct.SellerId);
        Assert.Equal(productId, savedProduct.Id);
        Assert.Equal(subCategoryId, savedProduct.SubCategoryId);
        Assert.Equal(now, savedProduct.CreatedAt);
        
        // Act
        repository.Remove(product);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Products
            .FirstOrDefaultAsync(x => 
                x.Id == product.Id);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Should_Throws_When_Product_Is_Null()
    {
        // Arrange
        var sellerId = 23242;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductProjection(_context, eventStoreMock.Object);

        Product product = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            repository.Remove(product));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/