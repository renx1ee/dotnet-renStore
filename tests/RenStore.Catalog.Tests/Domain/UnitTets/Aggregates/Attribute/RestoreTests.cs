using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Attribute;

public class RestoreTests : AttributeTestBase
{
    [Fact]
    public void Should_Raise_Restored_Event()
    {
        // Arrange
        var deleteNow = DateTimeOffset.Now;
        var restoreNow = deleteNow.AddMinutes(1);
        var attribute = CreateAttribute();
        
        // Act
        attribute.Delete(deleteNow);
        attribute.UncommittedEventsClear();
        attribute.Restore(restoreNow);

        var @event = Assert.Single(
            attribute.GetUncommittedEvents());
        var result = Assert.IsType<AttributeRestoredEvent>(@event);
        
        // Assert: result
        Assert.Equal(restoreNow, result.OccurredAt);
        // Assert: event
        Assert.False(attribute.IsDeleted);
        Assert.Null(attribute.DeletedAt);
        Assert.Equal(restoreNow, attribute.UpdatedAt);
        Assert.Equal(attribute.Id, result.AttributeId);
    }
    
    [Fact]
    public void Should_Exception_When_IsNotDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.Restore(now));
    }
}