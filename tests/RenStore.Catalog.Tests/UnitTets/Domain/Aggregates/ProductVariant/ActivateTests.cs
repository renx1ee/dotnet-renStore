using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.ProductVariant;

public class ActivateTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Raise_VariantPublished_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.Activate(now);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantPublished>(@event);

        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(ProductVariantStatus.Published, variant.Status);
    }
    
    [Fact]
    public void Should_NoRaise_When_StatusIsAlreadyPublished()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        
        var variant = CreateValidProductVariant();

        // Act
        variant.Activate(now);
        variant.UncommittedEventsClear();
        variant.Activate(now);
        
        // Assert
        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_When_VariantIsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        
        var variant = CreateValidProductVariant();
        variant.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            variant.Activate(now));
    }
}