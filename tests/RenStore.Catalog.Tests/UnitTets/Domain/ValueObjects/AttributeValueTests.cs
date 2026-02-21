using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.ValueObjects;

public class AttributeValueTests
{
    private const string MaxValue =
        "ValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValue";
    
    [Theory]
    [InlineData("Test Value")]
    [InlineData(" Test Value ")]
    public void Create_Should_CreatedAttribute(
        string value)
    {
        // Arrange
        var trimmedValue = "Test Value";

        // Act
        var result = AttributeValue.Create(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trimmedValue, result.Value);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxValue)]
    public void Create_Should_Throw_Where_ValueIsIncorrect(
        string value)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            AttributeValue.Create(value));
    }
    
    [Theory]
    [InlineData("Test Value")]
    [InlineData(" Test Value ")]
    public void ValueNormalizeAndValidate_Should_CreatedAttribute(
        string value)
    {
        // Arrange
        var trimmedValue = "Test Value";

        // Act
        var result = AttributeValue
            .ValueNormalizeAndValidate(value);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trimmedValue, result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxValue)]
    public void ValueNormalizeAndValidate_Should_Throw_Where_ValueIsIncorrect(
        string value)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            AttributeValue.ValueNormalizeAndValidate(value));
    }
}