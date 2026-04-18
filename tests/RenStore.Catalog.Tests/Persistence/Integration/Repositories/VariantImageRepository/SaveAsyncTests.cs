/*using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantImageRepository;

[Collection("sequential")]
public sealed class SaveAsyncTests : IAsyncLifetime
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
    public async Task Should_Save_ProductVariant_To_Postgres()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var weight = 200;
        var height = 200;
        var sortOrder = 20;
        var fileSizeBytes = 20552530;
        var originalFileName = Guid.NewGuid().ToString();
        var storagePath = Guid.NewGuid().ToString();
        var now = DateTimeOffset.UtcNow;
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var imageRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantImageRepository(eventStore, mediatorMock.Object);
        
        var product = VariantImage.Create(
            variantId: variantId,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            isMain: true,
            sortOrder: sortOrder,
            weight: weight, 
            height: height, 
            now: now);

        // Act
        await imageRepository.SaveAsync(product, CancellationToken.None);

        // Assert
        var productEvents = await _context.Events
            .Where(x => x.AggregateId == product.Id)
            .ToListAsync();
        
        Assert.NotEmpty(productEvents);

        var savedProduct = await imageRepository.GetAsync(
            product.Id, CancellationToken.None);
        
        Assert.Equal(variantId, savedProduct.VariantId);
        Assert.Equal(originalFileName, savedProduct.OriginalFileName);
        Assert.Equal(storagePath, savedProduct.StoragePath);
        Assert.Equal(fileSizeBytes, savedProduct.FileSizeBytes);
        Assert.Equal(sortOrder, savedProduct.SortOrder);
        Assert.Equal(weight, savedProduct.Weight);
        Assert.Equal(height, savedProduct.Height);
        Assert.Equal(now, savedProduct.UploadedAt);
        Assert.True(savedProduct.IsMain);
    }
    
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_ProductVariantIsNull()
    {
        // Arrange
        VariantImage image = null;

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var imageRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantImageRepository(eventStore, mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await imageRepository.SaveAsync(
                image: image!, 
                CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}*/