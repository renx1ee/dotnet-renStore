using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record MainImageIdSet(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId)
    : IDomainEvent;