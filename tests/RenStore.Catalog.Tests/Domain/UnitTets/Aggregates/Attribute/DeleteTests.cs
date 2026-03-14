using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Attribute;

public class DeleteTests : AttributeTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act
        attribute.UncommittedEventsClear();
        attribute.Delete(now);

        var @event = Assert.Single(
            attribute.GetUncommittedEvents());
        var result = Assert.IsType<AttributeRemovedEvent>(@event);
        
        // Assert: result
        Assert.Equal(now, result.OccurredAt);
        // Assert: event
        Assert.True(attribute.IsDeleted);
        Assert.Equal(now, attribute.DeletedAt);
        Assert.Equal(now, attribute.UpdatedAt);
    }
    
    [Fact]
    public void Should_Exception_When_AlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act
        attribute.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.Delete(now));
    }
}