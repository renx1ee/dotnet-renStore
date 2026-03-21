using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class SetMainImageIdTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Rise_Variant_Main_Image_Id_Set()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var imageId = Guid.NewGuid();
        
        var variant = CreateValidProductVariant();
        
        // Act
        variant.AddImageReference(
            now: now,
            imageId: imageId);
        
        variant.UncommittedEventsClear();
        
        variant.SetMainImageId(
            now: now,
            imageId: imageId);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<MainImageIdSetEvent>(@event);
        
        // Assert: event
        Assert.Equal(imageId, result.ImageId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variant.Id, result.VariantId);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(imageId, variant.MainImageId);
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Single(variant.ImageIds);
    }
    
    [Fact]
    public void Should_Return_When_Variant_Image_The_Same()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var imageId = Guid.NewGuid();
        
        var variant = CreateValidProductVariant();
        
        // Act
        variant.AddImageReference(
            now: now,
            imageId: imageId);
        
        variant.UncommittedEventsClear();
        
        variant.SetMainImageId(
            now: now,
            imageId: imageId);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<MainImageIdSetEvent>(@event);
        
        // Assert: event
        Assert.Equal(imageId, result.ImageId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(variant.Id, result.VariantId);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Single(variant.ImageIds);
        
        variant.SetMainImageId(
            now: now,
            imageId: imageId);

        variant.UncommittedEventsClear();
        
        Assert.Empty(variant.GetUncommittedEvents());
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
            variant.SetMainImageId(
                now: now,
                imageId: imageId));
    }
}