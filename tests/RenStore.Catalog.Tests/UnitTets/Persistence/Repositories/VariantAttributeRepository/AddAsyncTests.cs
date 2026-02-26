using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Persistence.Repositories.VariantAttributeRepository;

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
    public async Task Should_Save_Attribute_To_Postgres()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var key = "key";
        var value = "sample product attribute value";
        var now = DateTimeOffset.UtcNow;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantAttributeRepository(
                _context, 
                eventStoreMock.Object);

        var attribute = VariantAttribute.Create(
            now: now,
            variantId: variantId,
            key: key,
            value: value);

        // Act
        var resultId = await repository
            .AddAsync(attribute, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == resultId);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.Equal(attribute.Id, resultId);
        Assert.Equal(now, result.CreatedAt);
        Assert.Equal(key, result.Key);
        Assert.Equal(value, result.Value);
    }
    
    [Fact]
    public async Task Should_Throw_When_Attribute_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantAttributeRepository(
                _context, 
                eventStoreMock.Object);

        VariantAttribute attribute = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            repository.AddAsync(attribute!, CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}