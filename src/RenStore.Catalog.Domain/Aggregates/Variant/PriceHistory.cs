using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

public sealed class PriceHistory
{
    public Guid Id { get; private set; }
    public SharedKernal.Domain.ValueObjects.Price Price { get; private set; }
    public DateTimeOffset ValidFrom { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeactivatedAt { get; private set; }
    public Guid SizeId { get; private set; }
    
    private PriceHistory() { }

    internal PriceHistory(
        DateTimeOffset now,
        DateTimeOffset startDate,
        Guid priceId,
        Guid sizeId,
        decimal price,
        Currency currency)
    {
        Id = priceId;
        CreatedAt = now;
        ValidFrom = startDate;
        SizeId = sizeId;
        Price = new SharedKernal.Domain.ValueObjects.Price(
            amount: price,
            currency: currency);
        IsActive = true;
    }

    internal void Deactivate(
        DateTimeOffset validTo)
    {
        IsActive = false;
        DeactivatedAt = validTo;
    }
}