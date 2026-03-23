using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public sealed class DeleteTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        product.UncommittedEventsClear();
        
        // Act
        product.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductRemovedEvent>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.NotEqual(Guid.Empty, result.ProductId);
        
        // Assert: state
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(ProductStatus.Deleted, product.Status);
        Assert.Equal(now, product.DeletedAt);
        Assert.Equal(now, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        product.UncommittedEventsClear();
        
        // Act
        product.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        // Assert
        Assert.Throws<DomainException>(() =>
            product.Delete(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}