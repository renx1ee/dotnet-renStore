using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductRepository;

[Collection("sequential")]
public sealed class SaveAsyncTests : IAsyncLifetime
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
    public async Task Should_Save_Product_To_Postgres()
    {
        // Arrange
        var sellerId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var productRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(eventStore, mediatorMock.Object);
        
        var product = Product.Create(
            sellerId: sellerId,
            categoryId: categoryId,
            subCategoryId: subCategoryId,
            now: now);

        // Act
        await productRepository.SaveAsync(product, CancellationToken.None);

        // Assert
        var productEvents = await _context.Events
            .Where(x => x.AggregateId == product.Id)
            .ToListAsync();
        
        Assert.NotEmpty(productEvents);

        var savedProduct = await productRepository.GetAsync(
            product.Id, CancellationToken.None);
        
        Assert.Equal(sellerId, savedProduct.SellerId);
        Assert.Equal(product.Id, savedProduct.Id);
        Assert.Equal(categoryId, savedProduct.CategoryId);
        Assert.Equal(subCategoryId, savedProduct.SubCategoryId);
        Assert.Equal(now, savedProduct.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_ProductIsNull()
    {
        // Arrange
        Product product = null;

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var productRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(eventStore, mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await productRepository.SaveAsync(
                product: product!, 
                CancellationToken.None));
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}