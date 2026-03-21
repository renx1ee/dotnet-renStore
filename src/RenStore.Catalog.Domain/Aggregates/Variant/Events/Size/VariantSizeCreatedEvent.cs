using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

public sealed record VariantSizeCreatedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid SizeId,
    Guid VariantId,
    LetterSize LetterSize,
    SizeSystem SizeSystem,
    SizeType SizeType)
    : IDomainEvent;