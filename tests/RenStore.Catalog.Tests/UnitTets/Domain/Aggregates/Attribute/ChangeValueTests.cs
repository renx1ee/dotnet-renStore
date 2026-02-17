using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Attribute;

public class ChangeValueTests : AttributeTestBase
{
    private const string MaxValueLength = 
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklfewfwezxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvb";
    
    [Theory]
    [InlineData("123", "123")]
    [InlineData("KEYwfwfw", "KEYwfwfw")]
    [InlineData(" KEYwfwfw ", "KEYwfwfw")]
    public void Should_Raise_ValueChanged_Event(
        string value, 
        string trimmedValue)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act
        attribute.UncommittedEventsClear();
        
        attribute.ChangeValue(
            value: value, 
            now: now);

        var @event = Assert.Single(
            attribute.GetUncommittedEvents());
        var result = Assert.IsType<AttributeValueUpdated>(@event);
        
        // Assert: result
        Assert.Equal(trimmedValue, result.Value);
        Assert.Equal(now, attribute.UpdatedAt);
        // Assert: event
        Assert.Equal(trimmedValue, attribute.Value);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxValueLength)]
    public void Should_Exception_When_OnWrongParameter(
        string value)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.ChangeValue(
                value: value, 
                now: now));
    }

    [Fact]
    public void Should_Exception_When_Deleted()
    {
        // Arrange
        var value = "fwfwwwaf";
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act
        attribute.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.ChangeValue(
                value: value, 
                now: now));
    }
}