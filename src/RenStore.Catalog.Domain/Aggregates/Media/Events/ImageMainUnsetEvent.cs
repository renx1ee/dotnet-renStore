using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

public sealed record ImageMainUnsetEvent(
    Guid EventId,
    Guid VariantId,
    DateTimeOffset OccurredAt,
    Guid ImageId)
    : IDomainEvent;