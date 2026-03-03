using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class ChangeSortOrderTests : ImageTestBase
{
    [Fact]
    public void Should_Raise_SortOrderUpdated_Event()
    {
        // Arrange
        short newSortOrder = 232;
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        // Act
        image.ChangeSortOrder(
            now: now, 
            sortOrder: newSortOrder);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var result = Assert.IsType<ImageSortOrderUpdated>(@event);

        // Assert: event
        Assert.NotNull(result);
        Assert.Equal(newSortOrder, result.SortOrder);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(newSortOrder, image.SortOrder);
        Assert.Equal(now, image.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRaise_Where_SortOrderTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.UncommittedEventsClear();

        // Act
        image.ChangeSortOrder(
            now: now, 
            sortOrder: image.SortOrder);

        // Assert
        Assert.Empty(image.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_Where_ImageIsDeleted()
    {
        // Arrange
        short newSortOrder = 232;
        var now = DateTimeOffset.UtcNow;
        
        var image = CreateValidImage();
        image.Delete(now);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            image.ChangeSortOrder(
                now: now,
                sortOrder: newSortOrder));
    }
}