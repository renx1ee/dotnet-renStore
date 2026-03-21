using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public sealed record VariantCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ProductId,
    int ColorId,
    string Name,
    string NormalizedName,
    SizeSystem SizeSystem,
    SizeType SizeType,
    long Article,
    ProductVariantStatus Status,
    string Url)
    : IDomainEvent;