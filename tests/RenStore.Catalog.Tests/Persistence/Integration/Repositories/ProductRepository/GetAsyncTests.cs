using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using Xunit.Abstractions;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductRepository;

[Collection("sequential")]
public sealed class GetAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    private readonly ITestOutputHelper testOutputHelper;
    
    public GetAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }
    
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
    public async Task Should_Getting_Products_From_Postgres()
    {
        // Arrange
        var sellerId1 = Guid.NewGuid();
        var categoryId1 = Guid.NewGuid();
        var subCategoryId1 = Guid.NewGuid();
        var now1 = DateTimeOffset.UtcNow;
        
        var sellerId2 = Guid.NewGuid();
        var categoryId2 = Guid.NewGuid();
        var subCategoryId2 = Guid.NewGuid();
        var now2 = DateTimeOffset.UtcNow.AddHours(1);
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var productRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(eventStore, mediatorMock.Object);

        var product1 = Product.Create(
            sellerId: sellerId1,
            categoryId: categoryId1,
            subCategoryId: subCategoryId1,
            now: now1);
        
        var product2 = Product.Create(
            sellerId: sellerId2,
            categoryId: categoryId2,
            subCategoryId: subCategoryId2,
            now: now2);
        
        
        testOutputHelper.WriteLine($"Product1 ID before save: {product1.Id}");
        testOutputHelper.WriteLine($"Product2 ID before save: {product2.Id}");
        
        await productRepository.SaveAsync(product1, CancellationToken.None);
        await productRepository.SaveAsync(product2, CancellationToken.None);
        
        // Act
        var existingProduct1 = await productRepository
            .GetAsync(
                id: product1.Id,
                cancellationToken: CancellationToken.None);
        
        var existingProduct2 = await productRepository
            .GetAsync(
                id: product2.Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        Assert.NotNull(existingProduct1); // TODO:
        Assert.Equal(now1, existingProduct1.CreatedAt);
        Assert.Equal(sellerId1, existingProduct1.SellerId);
        Assert.Equal(categoryId1, existingProduct1.CategoryId);
        Assert.Equal(subCategoryId1, existingProduct1.SubCategoryId);
        
        Assert.NotNull(existingProduct2);
        Assert.Equal(now2, existingProduct2.CreatedAt);
        Assert.Equal(sellerId2, existingProduct2.SellerId);
        Assert.Equal(categoryId2, existingProduct2.CategoryId);
        Assert.Equal(subCategoryId2, existingProduct2.SubCategoryId);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_ProductNotFound()
    {
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var productRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductRepository(eventStore, mediatorMock.Object);
        
        var result = await productRepository.GetAsync(
            Guid.NewGuid(), CancellationToken.None);
    
        Assert.Null(result);
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}