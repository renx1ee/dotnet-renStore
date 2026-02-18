using RenStore.Catalog.Domain.Aggregates.Attribute;
using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Attribute;

public class CreateTests
{
    private const string MaxKeyLength = 
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbn";

    private const string MaxValueLength =
        "qwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvbnqwertyuiopasdfghjklzxcvb";
    
    [Fact]
    public void Should_Raise_AttributeCreated_Event()
    {
        // Arrange
        var variantId = Guid.NewGuid();
        var key = " qwertyuiopasdfg ";
        var trimmedKey = "qwertyuiopasdfg";
        var value = " qwertyuiopasdfg ";
        var trimmedValue = "qwertyuiopasdfg";
        var now = DateTimeOffset.Now;

        // Act
        var result = VariantAttribute.Create(
            now: now,
            variantId: variantId,
            key: key,
            value: value);

        var @event = Assert.Single(result.GetUncommittedEvents());
        var created = Assert.IsType<AttributeCreated>(@event);

        // Assert: Aggregate
        Assert.NotNull(result);
        Assert.Equal(variantId, result.VariantId);
        Assert.Equal(trimmedKey, result.Key);
        Assert.Equal(trimmedValue, result.Value);
        Assert.Equal(now, result.CreatedAt);
        Assert.NotEqual(Guid.Empty, result.Id);
        // Assert: Event
        Assert.Equal(variantId, created.VariantId);
        Assert.Equal(key, created.Key);
        Assert.Equal(value, created.Value);
        Assert.Equal(now, created.OccurredAt);
    }

    [Fact]
    public void Should_Exception_FailOnWrongVariantId()
    {
        // Arrange
        var variantId = Guid.Empty;
        var key = Guid.NewGuid().ToString();
        var value = Guid.NewGuid().ToString();
        var now = DateTimeOffset.Now;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantAttribute.Create(
                now: now,
                variantId: variantId,
                key: key,
                value: value));
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

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            VariantAttribute.Create(
                now: now,
                variantId: variantId,
                key: key,
                value: value));
    }
}