using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Product;

public class MarkAsApprovedTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Approved_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.UncommittedEventsClear();
        
        // Act
        product.MarkAsApproved(now);
        
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductApproved>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(product.UpdatedAt, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(ProductStatus.Approved, product.Status);
        Assert.Equal(now, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.Delete(now);
         
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            product.MarkAsApproved(now));
    }
}