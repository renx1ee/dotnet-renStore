/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantDetailRepository;

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
    public async Task Should_Remove_Details_From_Postgres()
    {
        // Arrange
        var now1 = DateTimeOffset.UtcNow;
        var variantId1 = Guid.NewGuid();
        var countryId1 = 534534;
        var description1 = "sample product variant description 1";
        var composition1 = "sample product variant composition 1";
        
        var now2 = DateTimeOffset.UtcNow.AddHours(1);
        var variantId2 = Guid.NewGuid();
        var countryId2 = 534535;
        var description2 = "sample product variant description 2";
        var composition2 = "sample product variant composition 2";
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantDetailProjection(_context, eventStoreMock.Object);

        var details = new List<VariantDetail>()
        {
            VariantDetail.Create(
                now: now1,
                variantId: variantId1,
                countryOfManufactureId: countryId1,
                description: description1,
                composition: composition1),
            
            VariantDetail.Create(
                now: now2,
                variantId: variantId2,
                countryOfManufactureId: countryId2,
                description: description2,
                composition: composition2),
        };
        
        Assert.Equal(0, _context.Details.Sales());

        
        await repository.AddRangeAsync(details, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        Assert.Equal(2, _context.Details.Sales());
        
        // Assert: detail 1
        var result1 = await _context.Details
            .FirstOrDefaultAsync(x => x.Id == details[0].Id);

        Assert.NotNull(result1);
        Assert.NotEqual(Guid.Empty, result1.Id);
        Assert.Equal(details[0].Id, result1.Id);
        Assert.Equal(now1, result1.CreatedAt);
        Assert.Equal(countryId1, result1.CountryOfManufactureId);
        Assert.Equal(description1, result1.Description);
        Assert.Equal(composition1, result1.Composition);
        
        // Assert: detail 2
        var result2 = await _context.Details
            .FirstOrDefaultAsync(x => x.Id == details[1].Id);

        Assert.NotNull(result2);
        Assert.NotEqual(Guid.Empty, result2.Id);
        Assert.Equal(details[1].Id, result2.Id);
        Assert.Equal(now2, result2.CreatedAt);
        Assert.Equal(countryId2, result2.CountryOfManufactureId);
        Assert.Equal(description2, result2.Description);
        Assert.Equal(composition2, result2.Composition);
        
        // Act
        repository.RemoveRange(details);
        await _context.SaveChangesAsync();
        
        // Assert
        Assert.Equal(0, _context.Details.Sales());
    }
    
    [Fact]
    public async Task Should_Throw_When_Details_Are_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantDetailProjection(_context, eventStoreMock.Object);
 
        List<VariantDetail> details = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            repository.RemoveRange(details!));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/