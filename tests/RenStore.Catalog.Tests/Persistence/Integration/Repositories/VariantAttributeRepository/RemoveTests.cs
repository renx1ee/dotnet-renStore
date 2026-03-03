using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantAttributeRepository;

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
    public async Task Should_Remove_Attribute_To_Postgres()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var key = "key";
        var value = "sample product attribute value";
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(
                _context, 
                eventStoreMock.Object);

        var attribute = VariantAttribute.Create(
            now: now,
            variantId: variantId,
            key: key,
            value: value);

        
        var resultId = await repository
            .AddAsync(attribute, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        var existingAttribute = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == resultId);

        Assert.NotNull(existingAttribute);
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.Equal(attribute.Id, resultId);
        Assert.Equal(now, existingAttribute.CreatedAt);
        Assert.Equal(key, existingAttribute.Key);
        Assert.Equal(value, existingAttribute.Value);
        
        // Act 
        repository.Remove(attribute);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == resultId);

        Assert.Null(result);
    }
    
    [Fact]
    public async Task Should_Throw_When_Attribute_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantAttributeProjection(
                _context, 
                eventStoreMock.Object);

        VariantAttribute attribute = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            repository.Remove(attribute!));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}