using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class DeleteTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
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
        
        // Act
        variant.UncommittedEventsClear();
        variant.RemoveAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<AttributeRemovedEvent>(@event);
        
        // Assert: result
        Assert.Equal(now, result.OccurredAt);
        // Assert: event
        Assert.True(variant.Attributes.ToList()[0].IsDeleted);
        Assert.Equal(now, variant.Attributes.ToList()[0].DeletedAt);
        Assert.Equal(now, variant.UpdatedAt);
    }
    
    [Fact]
    public void Should_Exception_When_AlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var value = "qwertyuiopasdfg";
        var key = " qwertyuiopasdfg ";
        
        var variant = CreateValidProductVariant();
        
        var attributeId = variant.AddAttribute(
            now: now,
            key: key,
            value: value);
        
        variant.UncommittedEventsClear();
        
        variant.UncommittedEventsClear();
        variant.RemoveAttribute(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            attributeId: attributeId);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.RemoveAttribute(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole,
                attributeId: attributeId));
    }
}