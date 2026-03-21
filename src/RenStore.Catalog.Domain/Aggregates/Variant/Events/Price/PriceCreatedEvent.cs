using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

public sealed record PriceCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    DateTimeOffset EffectiveFrom,
    Guid PriceId,
    Guid SizeId,
    Currency Currency,
    decimal PriceAmount)
    : IDomainEvent;