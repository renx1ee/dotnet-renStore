using RenStore.Catalog.Domain.Entities;
using RenStore.SharedKernal.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

public class ProductPriceHistory
    : EntityWithSoftDeleteBase
{
    public Guid Id { get; private set; }
    public decimal Price { get; private set; }
    public decimal OldPrice { get; private set; }
    public decimal DiscountPrice { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string ChangedBy { get; private set; } = string.Empty;
    public Guid ProductVariantId { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}