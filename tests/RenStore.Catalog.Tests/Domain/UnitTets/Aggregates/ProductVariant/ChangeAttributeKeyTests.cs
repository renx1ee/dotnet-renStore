using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ChangeAttributeKeyTests : ProductVariantTestBase
{
    private const string MaxKeyLength = 
        "qwertyuiofewwfpasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbn";
    
    [Theory]
    [InlineData("123", "123")]
    [InlineData("KEYwfwfw", "KEYwfwfw")]
    [InlineData(" KEYwfwfw ", "KEYwfwfw")]
    public void Should_Raise_KeyChanged_Event(
        string newKey, 
        string newTrimmedKey)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        
        var variant = CreateValidProductVariant();
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();
        
        // Act
        variant.ChangeAttributeKey(
            now: now,
            key: newKey,
            attributeId: attributeId);
        
        var @event = Assert.Single( variant.GetUncommittedEvents());
        var result = Assert.IsType<AttributeKeyUpdatedEvent>(@event);
        
        // Assert: result
        Assert.Equal(newTrimmedKey, result.Key);
        Assert.Equal(now, variant.UpdatedAt);
        // Assert: event
        Assert.Equal(newTrimmedKey, variant.Attributes.ToList()[0].Key);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxKeyLength)]
    public void Should_Exception_When_WrongParameter(
        string wrongKey)
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
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.ChangeAttributeKey(
                now: now,
                key: wrongKey,
                attributeId: attributeId));
    }
    
    [Fact]
    public void Should_Exception_When_Deleted()
    {
        // Arrange
        var key = "fwfwwwaf";
        var value = "qwertyuiopasdfg";
        var now = DateTimeOffset.Now;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        
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
            variant.ChangeAttributeKey(
                now: now,
                key: key,
                attributeId: attributeId));
    }
}