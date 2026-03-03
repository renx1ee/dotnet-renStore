using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.ValueObjects;

public class AttributeKeyTests
{
    private const string MaxKey =
        "ValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValueValue";
    
    [Theory]
    [InlineData("Test Key")]
    [InlineData(" Test Key ")]
    public void Create_Should_CreatedAttribute(
        string key)
    {
        // Arrange
        var trimmedKey = "Test Key";

        // Act
        var result = AttributeKey.Create(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trimmedKey, result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxKey)]
    public void Create_Should_Throw_Where_KeyIsIncorrect(
        string key)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            AttributeKey.Create(key));
    }
    
    [Theory]
    [InlineData("Test Key")]
    [InlineData(" Test Key ")]
    public void KeyNormalizeAndValidate_Should_CreatedAttribute(
        string key)
    {
        // Arrange
        var trimmedKey = "Test Key";

        // Act
        var result = AttributeKey
            .KeyNormalizeAndValidate(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trimmedKey, result);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(MaxKey)]
    public void KeyNormalizeAndValidate_Should_Throw_Where_KeyIsIncorrect(
        string key)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            AttributeKey.KeyNormalizeAndValidate(key));
    }
}