using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class RestoreTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Raise_Restored_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        // Act
        variant.RemoveAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);
        
        variant.UncommittedEventsClear();
        
        variant.RestoreAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<AttributeRestoredEvent>(@event);
        
        // Assert: result
        Assert.Equal(now, result.OccurredAt);
        // Assert: event
        Assert.False(variant.Attributes.ToList()[0].IsDeleted);
        Assert.Null(variant.Attributes.ToList()[0].DeletedAt);
        Assert.Equal(now, variant.Attributes.ToList()[0].UpdatedAt);
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(result.AttributeId, variant.Attributes.ToList()[0].Id);
    }
    
    [Fact]
    public void Should_NoRise_When_IsNotDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();
        
        // Act & Assert
        variant.RestoreAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);
        
        Assert.Empty(variant.GetUncommittedEvents());
    }
}