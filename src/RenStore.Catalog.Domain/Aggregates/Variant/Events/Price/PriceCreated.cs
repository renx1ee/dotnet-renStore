using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

public record PriceCreated(
    DateTimeOffset OccurredAt,
    DateTimeOffset EffectiveFrom,
    Guid PriceId,
    Guid SizeId,
    Currency Currency,
    decimal PriceAmount);