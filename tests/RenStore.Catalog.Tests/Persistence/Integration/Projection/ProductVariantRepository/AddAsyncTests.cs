/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.ProductVariantRepository;

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
    public async Task Should_Save_Variant_To_Postgres()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var url = "https://renxstore.ru/catalog";
        var colorId = 2342;
        var name = "sample product variant name";
        var sizeType = SizeType.Clothes;
        var sizeSystem = SizeSystem.RU;
        var article = 4232255;
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductVariantProjection(
                _context, 
                eventStoreMock.Object);

        var variant = ProductVariant.Create(
            now: now,
            productId: variantId,
            colorId: colorId,
            name: name,
            sizeType: sizeType,
            sizeSystem: sizeSystem,
            article: article,
            url: url);

        // Act
        var resultId = await repository
            .AddAsync(variant, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Variants
            .FirstOrDefaultAsync(x => x.Id == variant.Id);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.Equal(variant.Id, resultId);
        Assert.Equal(now, result.CreatedAt);
        Assert.Equal(url, result.Url);
        Assert.Equal(colorId, result.ColorId);
        Assert.Equal(name, result.Name);
        Assert.Equal(sizeType, result.SizeType);
        Assert.Equal(sizeSystem, result.SizeSystem);
        Assert.Equal(article, result.Article);
    }
    
    [Fact]
    public async Task Should_Throw_When_Variant_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new ProductVariantProjection(_context, eventStoreMock.Object);

        ProductVariant variant = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            await repository.AddAsync(variant!, CancellationToken.None));
    }
    

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/