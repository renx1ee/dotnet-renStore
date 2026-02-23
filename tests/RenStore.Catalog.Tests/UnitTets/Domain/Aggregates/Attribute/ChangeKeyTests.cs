using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Attribute;

public class ChangeKeyTests : AttributeTestBase
{
    private const string MaxKeyLength = 
        "qwertyuiofewwfpasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbn";
    
    [Theory]
    [InlineData("123", "123")]
    [InlineData("KEYwfwfw", "KEYwfwfw")]
    [InlineData(" KEYwfwfw ", "KEYwfwfw")]
    public void Should_Raise_KeyChanged_Event(
        string key, 
        string trimmedKey)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var attribute = CreateAttribute();
        
        // Act
        attribute.UncommittedEventsClear();
        
        attribute.ChangeKey(
            key: key, 
            now: now);

        var @event = Assert.Single(
            attribute.GetUncommittedEvents());
        var result = Assert.IsType<AttributeKeyUpdated>(@event);
        
        // Assert: result
        Assert.Equal(trimmedKey, result.Key);
        Assert.Equal(now, attribute.UpdatedAt);
        // Assert: event
        Assert.Equal(trimmedKey, attribute.Key);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxKeyLength)]
    public void Should_Exception_When_WrongParameter(
        string key)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.ChangeKey(
                key: key, 
                now: now));
    }
    
    [Fact]
    public void Should_Exception_When_Deleted()
    {
        // Arrange
        var key = "fwfwwwaf";
        var now = DateTimeOffset.Now;
        var attribute = CreateAttribute();
        
        // Act
        attribute.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            attribute.ChangeKey(
                key: key, 
                now: now));
    }
}