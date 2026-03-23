using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class AddAttributeTests : ProductVariantTestBase
{
    private const string MaxKeyLength = 
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbn";

    private const string MaxValueLength =
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnxcvbnqwertyuiopasdfghjklzxcvbnqwqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvb";
    
    [Fact]
    public void Should_Raise_AttributeCreated_Event()
    {
        // Arrange
        var key = " qwertyuiopasdfg ";
        var trimmedKey = "qwertyuiopasdfg";
        var value = " qwertyuiopasdfg ";
        var trimmedValue = "qwertyuiopasdfg";
        var now = DateTimeOffset.Now;

        // Act
        var variant = CreateValidProductVariant();
        
        variant.UncommittedEventsClear();
        
        variant.AddAttribute(
            now: now,
            key: key,
            value: value);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var created = Assert.IsType<AttributeCreatedEvent>(@event);
        
        // Assert: Event
        Assert.Equal(variant.Id, created.VariantId);
        Assert.Equal(trimmedKey, created.Key);
        Assert.Equal(trimmedValue, created.Value);
        Assert.Equal(now, created.OccurredAt);
        
        // Assert: Aggregate
        var attributes = variant.Attributes.ToList();
        
        Assert.NotNull(variant);
        Assert.Equal(variant.Id, variant.Id);
        Assert.Equal(trimmedKey, attributes[0].Key);
        Assert.Equal(trimmedValue, attributes[0].Value);
        Assert.Equal(now, attributes[0].CreatedAt);
        Assert.NotEqual(Guid.Empty, attributes[0].Id);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("Key", "")]
    [InlineData("Key", " " )]
    [InlineData("", "Value")]
    [InlineData(" ", "Value")]
    [InlineData(MaxKeyLength, "Value")]
    [InlineData("Key", MaxValueLength)]
    public void CreateAttribute_Should_Exception_FailOnWrongParams(
        string key,
        string value)
    {
        // Arrange
        var variantId = Guid.Empty;
        var now = DateTimeOffset.Now;
        
        var variant = CreateValidProductVariant();
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.AddAttribute(
                now: now,
                key: key,
                value: value));
    }
}