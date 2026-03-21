using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class ArchiveTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Rise_Variant_Archived()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();
        
        // Act
        variant.Archive(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantArchivedEvent>(@event);
        
        // Assert: event
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(updatedById, result.UpdatedById);
        Assert.Equal(updatedByRole, result.UpdatedByRole);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(result.OccurredAt, variant.UpdatedAt);
        Assert.Equal(ProductVariantStatus.Archived, variant.Status);
        Assert.NotEqual(Guid.Empty, result.VariantId);
    }
    
    [Fact]
    public void Should_Return_When_Variant_Already_Archived()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.Archive(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantArchivedEvent>(@event);
        
        // Assert: event
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(updatedById, result.UpdatedById);
        Assert.Equal(updatedByRole, result.UpdatedByRole);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(result.OccurredAt, variant.UpdatedAt);
        Assert.Equal(ProductVariantStatus.Archived, variant.Status);
        Assert.NotEqual(Guid.Empty, result.VariantId);
        
        variant.UncommittedEventsClear();
        
        variant.Archive(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Is_Deleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
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
            variant.Archive(
                now: now,
                updatedById: updatedById,
                updatedByRole: updatedByRole));
    }
}