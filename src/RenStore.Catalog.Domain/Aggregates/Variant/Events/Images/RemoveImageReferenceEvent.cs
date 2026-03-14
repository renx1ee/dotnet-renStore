using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public sealed record RemoveImageReferenceEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId)
    : IDomainEvent;