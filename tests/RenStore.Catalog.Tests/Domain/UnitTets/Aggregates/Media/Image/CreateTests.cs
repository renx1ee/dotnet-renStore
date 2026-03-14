using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class CreateTests 
{
    private const string MaxPath = 
        "/storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1/";
    
    private const string MaxFileName = 
        "/storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1/";
    
    [Fact]
    public void Should_Raise_ImageCreated_Event()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;

        // Act
        var image = VariantImage.Create(
            now: now,
            variantId: variantId,
            originalFileName: "photo.jpg",
            storagePath: "/storage/products/2026/02/17/photo.jpg",
            fileSizeBytes: 1024,
            isMain: true,
            sortOrder: 1,
            weight: 800,
            height: 600);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var created = Assert.IsType<ImageCreatedEvent>(@event);

        // Assert: event
        Assert.Equal(image.Id, created.ImageId);
        Assert.Equal(variantId, created.VariantId);
        Assert.Equal("photo.jpg", created.OriginalFileName);
        Assert.Equal("/storage/products/2026/02/17/photo.jpg", created.StoragePath);
        Assert.Equal(1024, created.FileSizeBytes);
        Assert.Equal(1, created.SortOrder);
        Assert.Equal(800, created.Weight);
        Assert.Equal(600, created.Height);
        Assert.Equal(now, created.OccurredAt);
        Assert.True(created.IsMain);

        // Assert: state
        Assert.Equal(created.ImageId, image.Id);
        Assert.Equal(created.VariantId, image.VariantId);
        Assert.Equal(created.StoragePath, image.StoragePath);
        Assert.Equal(created.SortOrder, image.SortOrder);
    }
    
    [Fact]
    public void Should_Throw_When_VariantIdIsEmpty()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.Empty,
                originalFileName: "photo.jpg",
                storagePath: "/storage/products/2026/02/17/photo.jpg",
                fileSizeBytes: 1024,
                isMain: false,
                sortOrder: 1,
                weight: 800,
                height: 600));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxFileName)]
    public void Should_Throw_When_OriginalFileNameIncorrect(
        string originalFileName)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.Empty,
                originalFileName: originalFileName,
                storagePath: "/storage/products/2026/02/17/photo.jpg",
                fileSizeBytes: 1024,
                isMain: false,
                sortOrder: 1,
                weight: 800,
                height: 600));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(60 * 1024 * 1024)]
    public void Should_Throw_When_FileSizeIsInvalid(
        long fileSize)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.NewGuid(),
                originalFileName: "photo.jpg",
                storagePath: "/storage/products/2026/02/17/photo.jpg",
                fileSizeBytes: fileSize,
                isMain: false,
                sortOrder: 1,
                weight: 800,
                height: 600));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public void Should_Throw_When_SortOrderIsInvalid(short sortOrder)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.NewGuid(),
                originalFileName: "photo.jpg",
                storagePath: "/storage/products/2026/02/17/photo.jpg",
                fileSizeBytes: 1024,
                isMain: false,
                sortOrder: sortOrder,
                weight: 800,
                height: 600));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("min path")]
    [InlineData(MaxPath)]
    public void Should_Throw_Where_StoragePathIsIncorrect(
        string storagePath)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.NewGuid(),
                originalFileName: "photo.jpg",
                storagePath: storagePath,
                fileSizeBytes: 1024,
                isMain: false,
                sortOrder: 5,
                weight: 800,
                height: 600));
    }
    
    [Theory]
    [InlineData(-1, 1000)]
    [InlineData(0, 1000)]
    [InlineData(49, 1000)]
    [InlineData(500, -1)]
    [InlineData(500, 0)]
    [InlineData(500, 5001)]
    public void Should_Throw_Where_ParametersAreWrong(
        int weight,
        int height)
    {
        // Arrange
        var now = DateTimeOffset.Now;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantImage.Create(
                now: now,
                variantId: Guid.NewGuid(),
                originalFileName: "photo.jpg",
                storagePath: "storagePath/test/test",
                fileSizeBytes: 1024,
                isMain: false,
                sortOrder: 5,
                weight: weight,
                height: height));
    }
}