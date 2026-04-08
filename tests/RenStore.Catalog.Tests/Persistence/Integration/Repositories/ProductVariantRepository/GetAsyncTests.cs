using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using Xunit.Abstractions;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductVariantRepository;

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
    public async Task Should_Getting_ProductVariants_From_Postgres()
    {
        // Arrange
        var productId1 = Guid.NewGuid();
        var colorId1 = 1;
        var name1 = Guid.NewGuid().ToString();
        var url1 = Guid.NewGuid().ToString();
        var sizeType1 = SizeType.Clothes;
        var sizeSystem1 = SizeSystem.EU;
        var article1 = 42434224;
        var now1 = DateTimeOffset.UtcNow;
        
        var productId2 = Guid.NewGuid();
        var colorId2 = 1;
        var name2 = Guid.NewGuid().ToString();
        var url2 = Guid.NewGuid().ToString();
        var sizeType2 = SizeType.Clothes;
        var sizeSystem2 = SizeSystem.EU;
        var article2 = 42434224;
        var now2 = DateTimeOffset.UtcNow;
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var productVariantRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(eventStore, mediatorMock.Object);
        
        var variant1 = ProductVariant.Create(
            productId: productId1,
            colorId: colorId1,
            name: name1,
            sizeSystem: sizeSystem1,
            sizeType: sizeType1,
            article: article1,
            url: url1, 
            now: now1);
        
        var variant2 = ProductVariant.Create(
            productId: productId2,
            colorId: colorId2,
            name: name2,
            sizeSystem: sizeSystem2,
            sizeType: sizeType2,
            article: article2,
            url: url2, 
            now: now2);
        
        testOutputHelper.WriteLine($"Variant 1 ID before save: {variant1.Id}");
        testOutputHelper.WriteLine($"Variant 2 ID before save: {variant2.Id}");
        
        await productVariantRepository.SaveAsync(variant1, CancellationToken.None);
        await productVariantRepository.SaveAsync(variant2, CancellationToken.None);
        
        // Act
        var existingVariant1 = await productVariantRepository
            .GetAsync(
                id: variant1.Id,
                cancellationToken: CancellationToken.None);
        
        var existingVariant2 = await productVariantRepository
            .GetAsync(
                id: variant2.Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        Assert.NotNull(existingVariant1); // TODO:
        Assert.Equal(productId2, existingVariant2.ProductId);
        Assert.Equal(colorId2, existingVariant2.ColorId);
        Assert.Equal(name2, existingVariant2.Name);
        Assert.Equal(sizeSystem2, existingVariant2.SizeSystem);
        Assert.Equal(sizeType2, existingVariant2.SizeType);
        Assert.Equal(article2, existingVariant2.Article);
        Assert.Equal(url2, existingVariant2.Url);
        Assert.Equal(now2, existingVariant2.CreatedAt);
        
        Assert.NotNull(existingVariant2);
        Assert.Equal(productId2, existingVariant2.ProductId);
        Assert.Equal(colorId2, existingVariant2.ColorId);
        Assert.Equal(name2, existingVariant2.Name);
        Assert.Equal(sizeSystem2, existingVariant2.SizeSystem);
        Assert.Equal(sizeType2, existingVariant2.SizeType);
        Assert.Equal(article2, existingVariant2.Article);
        Assert.Equal(url2, existingVariant2.Url);
        Assert.Equal(now2, existingVariant2.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_ProductVariantNotFound()
    {
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var variantRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(eventStore, mediatorMock.Object);
        
        var result = await variantRepository.GetAsync(
            Guid.NewGuid(), CancellationToken.None);
    
        Assert.Null(result);
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}