using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class DeleteTests : ImageTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act
        image.Delete(now);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var result = Assert.IsType<ImageRemovedEvent>(@event);
        
        // Assert: event
        Assert.NotEqual(Guid.Empty, result.ImageId);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(image.Id, result.ImageId);
        Assert.True(image.IsDeleted);
        Assert.Equal(now, image.DeletedAt);
        Assert.Equal(now, image.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_ImageIsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        image.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.Delete(now));
    }
}