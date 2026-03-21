using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class ChangeAttributeValueTests : ProductVariantTestBase
{
    private const string MaxValueLength = 
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklfewfwezxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvb";
    
    [Theory]
    [InlineData("123", "123")]
    [InlineData("KEYwfwfw", "KEYwfwfw")]
    [InlineData(" KEYwfwfw ", "KEYwfwfw")]
    public void Should_Raise_ValueChanged_Event(
        string newValue, 
        string newTrimmedValue)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        
        var variant = CreateValidProductVariant();
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();
        
        variant.ChangeAttributeValue(
            attributeId: attributeId,
            value: newValue, 
            now: now);

        var @event = Assert.Single( variant.GetUncommittedEvents());
        var result = Assert.IsType<AttributeValueUpdatedEvent>(@event);
        
        // Assert: result
        Assert.Equal(newTrimmedValue, result.Value);
        Assert.Equal(now, variant.UpdatedAt);
        // Assert: event
        Assert.Equal(newTrimmedValue, variant.Attributes.ToList()[0].Value);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxValueLength)]
    public void Should_Exception_When_OnWrongParameter(
        string newValue)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var variant = CreateValidProductVariant();
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.ChangeAttributeValue(
                attributeId: attributeId,
                value: newValue, 
                now: now));
    }

    [Fact]
    public void Should_Exception_When_Deleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var variant = CreateValidProductVariant();
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();
        
        // Act
        variant.RemoveAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.ChangeAttributeValue(
                attributeId: attributeId,
                value: value, 
                now: now));
    }
}