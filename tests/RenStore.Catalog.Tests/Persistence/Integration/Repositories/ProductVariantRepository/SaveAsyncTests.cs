/*using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductVariantRepository;

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
    public async Task Should_Save_ProductVariant_To_Postgres()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var colorId = 1;
        var name = Guid.NewGuid().ToString();
        var url = Guid.NewGuid().ToString();
        var sizeType = SizeType.Clothes;
        var sizeSystem = SizeSystem.EU;
        var article = 42434224;
        var now = DateTimeOffset.UtcNow;
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var productVariantRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(eventStore, mediatorMock.Object);
        
        var variant = ProductVariant.Create(
            productId: productId,
            colorId: colorId,
            name: name,
            sizeSystem: sizeSystem,
            sizeType: sizeType,
            article: article,
            url: url, 
            now: now);

        // Act
        await productVariantRepository.SaveAsync(variant, CancellationToken.None);

        // Assert
        var productEvents = await _context.Events
            .Where(x => x.AggregateId == variant.Id)
            .ToListAsync();
        
        Assert.NotEmpty(productEvents);

        var savedProduct = await productVariantRepository.GetAsync(
            variant.Id, CancellationToken.None);
        
        Assert.Equal(productId, savedProduct.ProductId);
        Assert.Equal(colorId, savedProduct.ColorId);
        Assert.Equal(name, savedProduct.Name);
        Assert.Equal(sizeSystem, savedProduct.SizeSystem);
        Assert.Equal(sizeType, savedProduct.SizeType);
        Assert.Equal(article, savedProduct.Article);
        Assert.Equal(url, savedProduct.Url);
        Assert.Equal(now, savedProduct.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_ProductVariantIsNull()
    {
        // Arrange
        ProductVariant variant = null;

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var variantRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(eventStore, mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await variantRepository.SaveAsync(
                productVariant: variant!, 
                CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}*/