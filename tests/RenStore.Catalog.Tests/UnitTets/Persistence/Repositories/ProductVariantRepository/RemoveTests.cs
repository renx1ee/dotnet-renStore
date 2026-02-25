using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Persistence.Repositories.ProductVariantRepository;

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
    public async Task Should_Remove_Variant_From_Postgres()
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
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(
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
        
        var resultId = await repository
            .AddAsync(variant, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Act
        repository.Remove(variant);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Variants
            .FirstOrDefaultAsync(x => x.Id == variant.Id);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task Should_Throw_When_Variant_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(_context, eventStoreMock.Object);

        ProductVariant variant = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            repository.Remove(variant!));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}