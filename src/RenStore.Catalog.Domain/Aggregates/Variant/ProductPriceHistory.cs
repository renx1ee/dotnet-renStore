using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

public class ProductPriceHistory
{
    public Guid Id { get; private set; }
    public decimal Price { get; private set; }
    public decimal OldPrice { get; private set; }
    public decimal DiscountPrice { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset? EndDate { get; private set; }
    public string ChangedBy { get; private set; } = string.Empty;
    public Guid ProductVariantId { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}