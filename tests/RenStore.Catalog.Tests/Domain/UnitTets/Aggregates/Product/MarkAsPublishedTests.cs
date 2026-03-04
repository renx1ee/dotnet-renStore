using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class MarkAsPublishedTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Published_Event()
    {
        // Arrange
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        var publishedNow = DateTimeOffset.Now.AddHours(1);
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        product.UncommittedEventsClear();
        
        // Act
        product.MarkAsPublished(publishedNow);

        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductPublishedEvent>(@event);
        
        // Assert: event
        Assert.Equal(publishedNow, result.OccurredAt);
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(product.UpdatedAt, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(ProductStatus.Published, product.Status);
        Assert.Equal(publishedNow, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_HasNoVariants()
    {
        // Arrange
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        var publishedNow = DateTimeOffset.Now.AddHours(1);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.MarkAsPublished(publishedNow));
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyPublished()
    {
        // Arrange
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        var publishedNow = DateTimeOffset.Now.AddHours(1);
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        product.MarkAsPublished(publishedNow);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.MarkAsPublished(publishedNow));
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
    {
        // Arrange
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        var deleteAt = DateTimeOffset.Now.AddHours(1);
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        product.Delete(deleteAt);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.MarkAsPublished(deleteAt));
    }
}