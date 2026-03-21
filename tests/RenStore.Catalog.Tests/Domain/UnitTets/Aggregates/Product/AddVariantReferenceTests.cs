using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class AddVariantReferenceTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_ReferenceCreated_Event()
    {
        // Arrange
        var product = CreateProduct();
        var now = DateTimeOffset.Now;
        var variantId = Guid.NewGuid();

        product.UncommittedEventsClear();
        
        // Act
        product.AddVariantReference(
            variantId: variantId,
            now: now);
        
        // Assert: events
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductVariantReferenceCreatedEvent>(@event);
        
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variantId, result.VariantId);
        Assert.Equal(product.Id, result.ProductId);
        
        // Assert: state
        Assert.Single(product.ProductVariantIds);
        Assert.Contains(
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
            product.AddVariantReference(
                variantId: variantId,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_When_VariantIdAlreadyExists()
    {
        // Arrange
        var product = CreateProduct();
        var now = DateTimeOffset.Now;
        var variantId = Guid.NewGuid();
        
        product.AddVariantReference(
            variantId: variantId,
            now: now);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.AddVariantReference(
                variantId: variantId,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_When_ProductIsDeleted()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var product = CreateProduct();
        var now = DateTimeOffset.Now;
        var variantId = Guid.NewGuid();
        
        // Act
        product.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.AddVariantReference(
                variantId: variantId,
                now: now));
    }
    
    // тест на удаленном продукте
}