using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Tests.UnitTets.Persistence.Repositories;

namespace RenStore.Catalog.Tests.Integration.Persistence.Repositories.ProductVariantRepository;

public class RemoveRangeTests : IAsyncLifetime
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
    public async Task Should_Remove_Variants_From_Postgres()
    {
        // Arrange
        var variantId1 = Guid.NewGuid();
        var url1 = "https://renxstore.ru/catalog";
        var colorId1 = 2342;
        var name1 = "sample product variant name 1";
        var sizeType1 = SizeType.Clothes;
        var sizeSystem1 = SizeSystem.RU;
        var article1 = 4232255;
        var now1 = DateTimeOffset.UtcNow;
        
        var variantId2 = Guid.NewGuid();
        var url2 = "https://renxstore.ru/catalog";
        var colorId2 = 3535;
        var name2 = "sample product variant name 2";
        var sizeType2 = SizeType.Shoes;
        var sizeSystem2 = SizeSystem.EU;
        var article2 = 858452;
        var now2 = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(
                _context, 
                eventStoreMock.Object);

        var variants = new List<ProductVariant>()
        {
            ProductVariant.Create(
                now: now1,
                productId: variantId1,
                colorId: colorId1,
                name: name1,
                sizeType: sizeType1,
                sizeSystem: sizeSystem1,
                article: article1,
                url: url1),
            
            ProductVariant.Create(
                now: now2,
                productId: variantId2,
                colorId: colorId2,
                name: name2,
                sizeType: sizeType2,
                sizeSystem: sizeSystem2,
                article: article2,
                url: url2)
        };

        // Act
        await repository.AddRangeAsync(variants, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        Assert.Equal(2, _context.Variants.Count());
        
        // Act
        repository.RemoveRange(variants);
        await _context.SaveChangesAsync();
        
        // Assert
        Assert.Equal(0, _context.Variants.Count());
    }
    
    [Fact]
    public async Task Should_Throw_When_Variants_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .ProductVariantRepository(_context, eventStoreMock.Object);

        List<ProductVariant> variants = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            repository.RemoveRange(variants!));
    }
    
    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}