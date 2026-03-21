using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class RestoreTests : ImageTestBase
{
    [Fact]
    public void Should_Raise_Removed_Event()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.UtcNow;

        var image = CreateValidImage();
        image.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        image.UncommittedEventsClear();

        // Act
        image.Restore(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var result = Assert.IsType<ImageRestoredEvent>(@event);

        // Assert: event
        Assert.NotEqual(Guid.Empty, result.ImageId);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(image.Id, result.ImageId);
        Assert.False(image.IsDeleted);
        Assert.Null(image.DeletedAt);
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

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.Restore(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}