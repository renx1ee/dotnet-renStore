using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.Product;

public class DeleteTests : ProductTestBase
{
    [Fact]
    public void Should_Raise_Deleted_Event()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.UncommittedEventsClear();
        
        // Act
        product.Delete(now);
        var @event = Assert.Single(product.GetUncommittedEvents());
        var result = Assert.IsType<ProductRemoved>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.NotEqual(Guid.Empty, result.ProductId);
        
        // Assert: state
        Assert.Equal(product.Id, result.ProductId);
        Assert.Equal(ProductStatus.IsDeleted, product.Status);
        Assert.Equal(now, product.DeletedAt);
        Assert.Equal(now, product.UpdatedAt);
    }
    
    [Fact]
    public void Should_Throw_Where_IsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var product = CreateProduct();
        
        product.UncommittedEventsClear();
        
        // Act
        product.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() =>
            product.Delete(now));
    }
}