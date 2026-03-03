using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantAttributeRepository;

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
    public async Task Should_Remove_Attributes_To_Postgres()
    {
        // Arrange
        var variantId1 = Guid.NewGuid();
        var key1 = "key";
        var value1 = "sample product attribute value";
        var now1 = DateTimeOffset.UtcNow;
        
        var variantId2 = Guid.NewGuid();
        var key2 = "key";
        var value2 = "sample product attribute value";
        var now2 = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(
                _context, 
                eventStoreMock.Object);

        var attributes = new List<VariantAttribute>()
        {
            VariantAttribute.Create(
                now: now1,
                variantId: variantId1,
                key: key1,
                value: value1),
            
            VariantAttribute.Create(
                now: now2,
                variantId: variantId2,
                key: key2,
                value: value2)
        };
        
        await repository.AddRangeAsync(attributes, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        Assert.Equal(2, _context.Attributes.Count());
        
        // Act
        repository.RemoveRange(attributes);
        await _context.SaveChangesAsync();
        
        // Assert
        Assert.Equal(0, _context.Attributes.Count());
    }
    
    [Fact]
    public async Task Should_Throw_When_Attributes_Are_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(
                _context, 
                eventStoreMock.Object);

        List<VariantAttribute> attributes = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            repository.RemoveRange(attributes!));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}