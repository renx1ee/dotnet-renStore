using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Persistence.Repositories.ProductRepository;

public class AddAsyncTests : IAsyncLifetime
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
    public async Task Should_Saved_Product_To_Postgres()
    {
        // Arrange
        var sellerId = 23242;
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(
                _context, 
                eventStoreMock.Object);

        var product = Product.Create(
            sellerId: sellerId,
            subCategoryId: subCategoryId,
            now: now);

        // Act
        var productId = await repository
            .AddAsync(product, CancellationToken.None);
        
        await _context.SaveChangesAsync();

        // Assert
        var savedProduct = await _context.Products
            .FirstOrDefaultAsync(x => 
                x.Id == product.Id);

        Assert.NotNull(savedProduct);
        Assert.Equal(sellerId, savedProduct.SellerId);
        Assert.Equal(productId, savedProduct.Id);
        Assert.Equal(subCategoryId, savedProduct.SubCategoryId);
        Assert.Equal(now, savedProduct.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throws_When_Product_Is_Null()
    {
        // Arrange
        var sellerId = 23242;
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(_context, eventStoreMock.Object);

        Product product = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await repository.AddAsync(product, CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}