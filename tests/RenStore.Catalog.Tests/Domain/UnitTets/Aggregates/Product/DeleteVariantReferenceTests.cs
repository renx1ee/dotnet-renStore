using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class DeleteVariantReferenceTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_ReferenceRemoved_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        product.UncommittedEventsClear();
        
        // Act
        product.RemoveVariantReference(
            variantId: variantId, 
            now: now); 
        
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductVariantReferenceRemoved>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variantId, result.VariantId);
        Assert.Equal(product.Id, result.ProductId);
        
        // Assert: state
        Assert.Empty(product.ProductVariantIds);
        Assert.DoesNotContain(
            product.ProductVariantIds,
            x => x == variantId);
    }
    
    [Fact]
    public void Should_Throw_When_VariantIdIncorrect()
    {
        // Arrange
        var product = CreateProduct();
        var now = DateTimeOffset.Now;
        var variantId = Guid.Empty;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.RemoveVariantReference(
                variantId: variantId,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_When_VariantIdDoesNotExists()
    {
        // Arrange
        var product = CreateProduct();
        var now = DateTimeOffset.Now;
        var variantId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.RemoveVariantReference(
                variantId: variantId,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        var variantId = Guid.NewGuid();
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        // Act
        product.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() =>
            product.RemoveVariantReference(
            now: now,  
            variantId: variantId));
    }
}