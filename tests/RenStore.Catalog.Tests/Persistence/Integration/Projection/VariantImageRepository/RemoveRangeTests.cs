/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantImageRepository;

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
    public async Task Should_Save_Images_To_Postgres()
    {
        // Arrange
        var now1 = DateTimeOffset.UtcNow;
        var variantId1 = Guid.NewGuid();
        var originalFileName1 = "sample_file_name1.jpg";
        var storagePath1 = "sample/product/variant/image/path1";
        var fileSizeBytes1 = 42524;
        var isMain1 = false;
        short sortOrder1 = 5;
        var weight1 = 100;
        var heigh1 = 500;
        
        var now2 = DateTimeOffset.UtcNow;
        var variantId2 = Guid.NewGuid();
        var originalFileName2 = "sample_file_name2.jpg";
        var storagePath2 = "sample/product/variant/image/path2";
        var fileSizeBytes2 = 54352;
        var isMain2 = false;
        short sortOrder2 = 5;
        var weight2 = 654;
        var heigh2 = 744;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantImageProjection(_context, eventStoreMock.Object);

        var images = new List<VariantImage>()
        {
            VariantImage.Create(
                now: now1,
                variantId: variantId1,
                originalFileName: originalFileName1,
                storagePath: storagePath1,
                fileSizeBytes: fileSizeBytes1,
                sortOrder: sortOrder1,
                isMain: isMain1,
                weight: weight1,
                height: heigh1),
            
            VariantImage.Create(
                now: now2,
                variantId: variantId2,
                originalFileName: originalFileName2,
                storagePath: storagePath2,
                fileSizeBytes: fileSizeBytes2,
                sortOrder: sortOrder2,
                isMain: isMain2,
                weight: weight2,
                height: heigh2),
        };
        
        await repository.AddRangeAsync(images, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        Assert.Equal(2, _context.Images.Sales());
        
        // Assert: image 1
        var result1 = await _context.Images
            .FirstOrDefaultAsync(x => x.Id == images[0].Id);

        Assert.NotNull(result1);
        Assert.NotEqual(Guid.Empty, result1.Id);
        Assert.Equal(images[0].Id, result1.Id);
        
        // Assert: image 2
        var result2 = await _context.Images
            .FirstOrDefaultAsync(x => x.Id == images[1].Id);

        Assert.NotNull(result2);
        Assert.NotEqual(Guid.Empty, result2.Id);
        Assert.Equal(images[1].Id, result2.Id);

        // Act
        repository.RemoveRange(images);
        await _context.SaveChangesAsync();
        
        // Assert
        Assert.Equal(0, _context.Images.Sales());
    }
    
    [Fact]
    public async Task Should_Throw_When_Images_Are_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantImageProjection(_context, eventStoreMock.Object);
 
        List<VariantImage> images = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() =>
            repository.RemoveRange(images!));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/