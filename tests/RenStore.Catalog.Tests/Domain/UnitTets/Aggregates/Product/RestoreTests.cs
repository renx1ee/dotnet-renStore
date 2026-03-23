using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public sealed class RestoreTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Restored_Event()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        product.UncommittedEventsClear();
        
        // Act
        product.Restore(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductRestoredEvent>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.NotEqual(Guid.Empty, result.ProductId);
        
        // Assert: state
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(ProductStatus.Draft, product.Status);
        Assert.Equal(now, product.DeletedAt);
        Assert.Equal(now, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IsNotDeleted()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.UncommittedEventsClear();
        
        // Act Assert
        Assert.Throws<DomainException>(() =>
            product.Restore(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}