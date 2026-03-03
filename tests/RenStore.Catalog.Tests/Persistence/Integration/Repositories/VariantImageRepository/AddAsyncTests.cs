/*using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantImageRepository;

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
    public async Task Should_Save_Image_To_Postgres()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var variantId = Guid.NewGuid();
        var originalFileName = "sample_file_name.jpg";
        var storagePath = "sample/product/variant/image/path";
        var fileSizeBytes = 42524;
        var isMain = false;
        short sortOrder = 5;
        var weight = 100;
        var heigh = 500;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantImageProjection(_context, eventStoreMock.Object);

        var image = VariantImage.Create(
            now: now,
            variantId: variantId,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            sortOrder: sortOrder,
            isMain: isMain,
            weight: weight,
            height: heigh);

        // Act
        var resultId = await repository
            .AddAsync(image, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        // Assert
        var result = await _context.Images
            .FirstOrDefaultAsync(x => x.Id == resultId);

        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, resultId);
        Assert.Equal(image.Id, resultId);
        Assert.Equal(now, result.UploadedAt);
        Assert.Equal(variantId, result.VariantId);
        Assert.Equal(originalFileName, result.OriginalFileName);
        Assert.Equal(storagePath, result.StoragePath);
        Assert.Equal(fileSizeBytes, result.FileSizeBytes);
        Assert.Equal(isMain, result.IsMain);
        Assert.Equal(sortOrder, result.SortOrder);
        Assert.Equal(weight, result.Weight);
        Assert.Equal(heigh, result.Height);
    }
    
    [Fact]
    public async Task Should_Throw_When_Image_Is_Null()
    {
        // Arrange
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new VariantImageProjection(_context, eventStoreMock.Object);
 
        VariantImage image = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            repository.AddAsync(image!, CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}*/