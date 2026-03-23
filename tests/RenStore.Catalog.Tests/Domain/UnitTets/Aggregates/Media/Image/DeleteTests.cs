using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public sealed class DeleteTests : ImageTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act
        image.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var result = Assert.IsType<VariantImageRemovedEvent>(@event);
        
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
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        image.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.Delete(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}