using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public sealed record AddedImageReferenceEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid ImageId,
    Guid VariantId)
    : IDomainEvent;