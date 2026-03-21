using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class AddSizeTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Rise_Variant_Size_Added()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();
        
        // Act
        var sizeId = variant.AddSize(
            now: now,
            letterSize: letterSize);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantSizeCreatedEvent>(@event);
        
        // Assert: event
        Assert.Equal(sizeId, result.SizeId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(letterSize, result.LetterSize);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(letterSize, result.LetterSize);
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Single(variant.Sizes, x => x.Id == sizeId);
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Size_Already_Exists()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.AddSize(
            now: now,
            letterSize: letterSize);

        Assert.Throws<DomainException>(() =>
            variant.AddSize(
                now: now,
                letterSize: letterSize));
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Is_Deleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        Assert.Throws<DomainException>(() =>
            variant.AddSize(
                now: now,
                letterSize: letterSize));
    }
}