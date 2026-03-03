using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class ChangeStoragePathTests : ImageTestBase
{
    private const string MaxPath = 
        "/storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1//storage/path/1/";
    
    [Fact]
    public void Should_Raise_StoragePathUpdated_Event()
    {
        // Arrange
        var newStoragePath = "/new/storage/path/";
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        // Act
        image.ChangeStoragePath(
            now: now, 
            storagePath: newStoragePath);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var result = Assert.IsType<ImageStoragePathUpdated>(@event);

        // Assert: event
        Assert.NotNull(result);
        Assert.Equal(newStoragePath, result.StoragePath);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(newStoragePath, image.StoragePath);
        Assert.Equal(now, image.UpdatedAt);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("min path")]
    [InlineData(MaxPath)]
    public void Should_Throw_Where_StoragePathIsIncorrect(
        string newStoragePath)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.ChangeStoragePath(
                now: now,
                storagePath: newStoragePath));
    }
    
    [Fact]
    public void Should_NoRaise_Where_StoragePathTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        // Act
        image.ChangeStoragePath(
            now: now, 
            storagePath: image.StoragePath);

        // Assert
        Assert.Empty(image.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_Where_ImageIsDeleted()
    {
        // Arrange
        var newStoragePath = "/new/storage/path/";
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.Delete(now);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.ChangeStoragePath(
                now: now,
                storagePath: newStoragePath));
    }
}