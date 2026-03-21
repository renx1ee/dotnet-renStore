using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class AddImageReferenceTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Rise_Variant_Image_Id_Added()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var imageId = Guid.NewGuid();
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();
        
        // Act
        variant.AddImageReference(
            now: now,
            imageId: imageId);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<AddedImageReferenceEvent>(@event);
        
        // Assert: event
        Assert.Equal(imageId, result.ImageId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variant.Id, result.VariantId);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Single(variant.ImageIds);
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Image_Already_Exists()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var imageId = Guid.NewGuid();
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();
        
        // Act
        variant.AddImageReference(
            now: now,
            imageId: imageId);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<AddedImageReferenceEvent>(@event);
        
        // Assert: event
        Assert.Equal(imageId, result.ImageId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variant.Id, result.VariantId);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Single(variant.ImageIds);

        Assert.Throws<DomainException>(() =>
            variant.AddImageReference(
                now: now,
                imageId: imageId));
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Image_Id_Is_Invalid()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var imageId = Guid.Empty;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();
        
        // Act & Arrange
        Assert.Throws<DomainException>(() =>
            variant.AddImageReference(
                now: now,
                imageId: imageId));
    }
}