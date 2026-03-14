using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class ChangeFileSizeBytesTests : ImageTestBase
{
    [Theory]
    [InlineData(479)]
    [InlineData(1000)]
    public void Should_Raise_FileSizeBytesUpdated_Event(
        long fileSizeBytes)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act
        image.ChangeFileSizeBytes(
            fileSizeBytes: fileSizeBytes, 
            now: now);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var created = Assert.IsType<ImageFileSizeBytesUpdatedEvent>(@event);
        
        // Assert: event
        Assert.Equal(fileSizeBytes, created.FileSizeBytes);
        Assert.Equal(now, created.OccurredAt);
        Assert.NotEqual(Guid.Empty, created.ImageId);
        
        // Assert: state
        Assert.Equal(image.Id, created.ImageId);
        Assert.Equal(fileSizeBytes, image.FileSizeBytes);
        Assert.Equal(now, image.UpdatedAt);
    }
    
    [Theory]
    [InlineData(50 * 1024 * 1024 + 1)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Throw_Where_ParametersAreWrong(
        long fileSizeBytes)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var image = CreateValidImage();
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            image.ChangeFileSizeBytes(
                fileSizeBytes: fileSizeBytes, 
                now: now));
    }
    
    [Fact]
    public void Should_Throw_Where_ParametersAreSame()
    {
        // Arrange
        var now = DateTimeOffset.Now;

        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act 
        image.ChangeFileSizeBytes(
            fileSizeBytes: image.FileSizeBytes,
            now: now);

        // Assert
        Assert.Empty(image.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_Where_ImageIsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var fileSizeBytes = 500;

        var image = CreateValidImage();
        image.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            image.ChangeFileSizeBytes(
                fileSizeBytes: fileSizeBytes, 
                now: now));
    }
}