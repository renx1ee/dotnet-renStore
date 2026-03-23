using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Product;

public sealed class CreateTests
{
    [Fact]
    public void Should_Raise_Created_Event()
    {
        // Arrange
        var sellerId = Guid.NewGuid();
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        
        // Act
        var result = Catalog.Domain.Aggregates.Product.Product.Create(
            sellerId: sellerId,
            subCategoryId: subCategoryId,
            now: now);

        var @event = Assert.Single(result.GetUncommittedEvents());
        var created = Assert.IsType<ProductCreatedEvent>(@event);

        // Assert: result
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(sellerId, result.SellerId);
        Assert.Equal(subCategoryId, result.SubCategoryId);
        Assert.Equal(now, result.CreatedAt);
        Assert.Equal(ProductStatus.PendingModeration, result.Status);
        Assert.Equal(result.Id, created.ProductId);
        Assert.Equal(result.CreatedAt, created.OccurredAt);
        // Assert: event
        Assert.NotEqual(Guid.Empty, created.ProductId);
        Assert.Equal(sellerId, created.SellerId);
        Assert.Equal(subCategoryId, created.SubCategoryId);
        Assert.Equal(now, created.OccurredAt);
        Assert.Equal(ProductStatus.PendingModeration, created.Status);
    }
    
    [Fact]
    public void Should_Throw_WhenSellerIdIsInvalid()
    {
        var sellerId = Guid.Empty;
        // Arrange
        var subCategoryId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        
        // Act
        Assert.Throws<DomainException>(() => 
            Catalog.Domain.Aggregates.Product.Product.Create(
                sellerId: sellerId,
                subCategoryId: subCategoryId,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_WhenSubCategoryIdIsInvalid()
    {
        // Arrange
        var subCategoryId = Guid.Empty;
        var sellerId = Guid.NewGuid();
        var now = DateTimeOffset.Now;
        
        // Act
        Assert.Throws<DomainException>(() => 
            Catalog.Domain.Aggregates.Product.Product.Create(
                sellerId: sellerId,
                subCategoryId: subCategoryId,
                now: now));
    }
}