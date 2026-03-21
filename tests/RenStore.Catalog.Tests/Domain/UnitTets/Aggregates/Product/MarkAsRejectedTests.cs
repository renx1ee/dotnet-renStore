using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public class MarkAsRejectedTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Rejected_Event()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.UncommittedEventsClear();
        
        // Act
        product.MarkAsRejected(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductRejectedEvent>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(product.UpdatedAt, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(ProductStatus.Rejected, product.Status);
        Assert.Equal(now, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
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
         
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.MarkAsRejected(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}