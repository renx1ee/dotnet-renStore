using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Repositories.ProductRepository;

public class RemoveTests : IAsyncLifetime
{
    private static string _connectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";
    
    private CatalogDbContext _context;

    [Fact]
    public async Task Should_Removed_Product_From_Postgres()
    {
        // Arrange
        var sellerId = 23242;
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: _connectionString)
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        /*await context.Database.MigrateAsync();*/

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Persistence.Write.Repositories.Postgresql
            .ProductRepository(
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
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: _connectionString)
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Persistence.Write.Repositories.Postgresql
            .ProductRepository(_context, eventStoreMock.Object);

        Product product = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            repository.Remove(product));
    }
    
    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}