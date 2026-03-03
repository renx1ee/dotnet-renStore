using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantAttributeRepository;

public class AddRangeAsyncTests : IAsyncLifetime
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
    public async Task Should_Save_Attributes_To_Postgres()
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

        // Act
        await repository.AddRangeAsync(attributes, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        Assert.Equal(2, _context.Attributes.Count());
        
        // Assert: attribute 1
        var existingAttribute1 = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == attributes[0].Id);

        Assert.NotNull(existingAttribute1);
        Assert.NotEqual(Guid.Empty, existingAttribute1.Id);
        Assert.Equal(attributes[0].Id, existingAttribute1.Id);
        Assert.Equal(now1, existingAttribute1.CreatedAt);
        Assert.Equal(key1, existingAttribute1.Key);
        Assert.Equal(value1, existingAttribute1.Value);
        
        // Assert: attribute 2
        var existingAttribute2 = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == attributes[1].Id);
        
        Assert.NotNull(existingAttribute2);
        Assert.NotEqual(Guid.Empty, existingAttribute2.Id);
        Assert.Equal(attributes[1].Id, existingAttribute2.Id);
        Assert.Equal(now2, existingAttribute2.CreatedAt);
        Assert.Equal(key2, existingAttribute2.Key);
        Assert.Equal(value2, existingAttribute2.Value);
    }
    
    [Fact]
    public async Task Should_Throw_When_Attributes_Are_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(_context, eventStoreMock.Object);

        List<VariantAttribute> attributes = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            repository.AddRangeAsync(attributes!, CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Not_Save_When_Categories_List_Count_0()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(_context, eventStoreMock.Object);

        var attributes = new List<VariantAttribute>();
        
        // Act
        await repository.AddRangeAsync(attributes, CancellationToken.None);
        
        // Assert
        Assert.Equal(0, await _context.Attributes.CountAsync());
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}