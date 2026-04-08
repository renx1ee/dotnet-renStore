/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantDetailRepository;

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
    public async Task Should_Save_Detail_To_Postgres()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var variantId = Guid.NewGuid();
        var countryId = 534534;
        var description = "sample product variant description";
        var composition = "sample product variant composition";
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantDetailProjection(_context, eventStoreMock.Object);

        var detail = VariantDetail.Create(
            now: now,
            variantId: variantId,
            countryOfManufactureId: countryId,
            description: description,
            composition: composition);

        // Act
        var resultId = await repository
            .AddAsync(detail, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Details
            .FirstOrDefaultAsync(x => x.Id == resultId);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.Equal(detail.Id, resultId);
        Assert.Equal(now, result.CreatedAt);
        Assert.Equal(countryId, result.CountryOfManufactureId);
        Assert.Equal(description, result.Description);
        Assert.Equal(composition, result.Composition);
    }
    
    [Fact]
    public async Task Should_Throw_When_Detail_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantDetailProjection(_context, eventStoreMock.Object);
 
        VariantDetail detail = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            repository.AddAsync(detail!, CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/