using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using Xunit.Abstractions;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.VariantImageRepository;

[Collection("sequential")]
public sealed class GetAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    private readonly ITestOutputHelper testOutputHelper;
    
    public GetAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }
    
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
    public async Task Should_Getting_ProductVariants_From_Postgres()
    {
        // Arrange
        var variantId1 = Guid.NewGuid();
        var weight1 = 200;
        var height1 = 200;
        var sortOrder1 = 20;
        var fileSizeBytes1 = 20552530;
        var originalFileName1 = Guid.NewGuid().ToString();
        var storagePath1 = Guid.NewGuid().ToString();
        var now1 = DateTimeOffset.UtcNow;
        
        var variantId2 = Guid.NewGuid();
        var weight2 = 200;
        var height2 = 200;
        var sortOrder2 = 20;
        var fileSizeBytes2 = 53523;
        var originalFileName2 = Guid.NewGuid().ToString();
        var storagePath2 = Guid.NewGuid().ToString();
        var now2 = DateTimeOffset.UtcNow.AddHours(1);
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var imageRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantImageRepository(eventStore, mediatorMock.Object);
        
        var image1 = VariantImage.Create(
            variantId: variantId1,
            originalFileName: originalFileName1,
            storagePath: storagePath1,
            fileSizeBytes: fileSizeBytes1,
            isMain: false,
            sortOrder: sortOrder1,
            weight: weight1, 
            height: height1, 
            now: now1);
        
        var image2 = VariantImage.Create(
            variantId: variantId2,
            originalFileName: originalFileName2,
            storagePath: storagePath2,
            fileSizeBytes: fileSizeBytes2,
            isMain: true,
            sortOrder: sortOrder2,
            weight: weight2, 
            height: height2, 
            now: now2);
        
        testOutputHelper.WriteLine($"Variant 1 ID before save: {image1.Id}");
        testOutputHelper.WriteLine($"Variant 2 ID before save: {image2.Id}");
        
        await imageRepository.SaveAsync(image1, CancellationToken.None);
        await imageRepository.SaveAsync(image2, CancellationToken.None);
        
        // Act
        var existingImage1 = await imageRepository
            .GetAsync(
                id: image1.Id,
                cancellationToken: CancellationToken.None);
        
        var existingImage2 = await imageRepository
            .GetAsync(
                id: image2.Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        Assert.NotNull(existingImage1);
        Assert.Equal(variantId1, existingImage1.VariantId);
        Assert.Equal(originalFileName1, existingImage1.OriginalFileName);
        Assert.Equal(storagePath1, existingImage1.StoragePath);
        Assert.Equal(fileSizeBytes1, existingImage1.FileSizeBytes);
        Assert.Equal(sortOrder1, existingImage1.SortOrder);
        Assert.Equal(weight1, existingImage1.Weight);
        Assert.Equal(height1, existingImage1.Height);
        Assert.Equal(now1, existingImage1.UploadedAt);
        Assert.False(existingImage1.IsMain);
        
        Assert.NotNull(existingImage2);
        Assert.Equal(variantId2, existingImage2.VariantId);
        Assert.Equal(originalFileName2, existingImage2.OriginalFileName);
        Assert.Equal(storagePath2, existingImage2.StoragePath);
        Assert.Equal(fileSizeBytes2, existingImage2.FileSizeBytes);
        Assert.Equal(sortOrder2, existingImage2.SortOrder);
        Assert.Equal(weight2, existingImage2.Weight);
        Assert.Equal(height2, existingImage2.Height);
        Assert.Equal(now2, existingImage2.UploadedAt);
        Assert.True(existingImage2.IsMain);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_ProductVariantNotFound()
    {
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var imageRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .VariantImageRepository(eventStore, mediatorMock.Object);
        
        var result = await imageRepository.GetAsync(
            Guid.NewGuid(), CancellationToken.None);
    
        Assert.Null(result);
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}