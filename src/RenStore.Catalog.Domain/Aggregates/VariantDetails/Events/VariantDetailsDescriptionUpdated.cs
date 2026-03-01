using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsDescriptionUpdated(
    Guid EventId,
    DateTimeOffset OccurredAt,
    string Description,
    Guid DetailId)
    : IDomainEvent;