using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsDescriptionUpdated(
    DateTimeOffset OccurredAt,
    string Description,
    Guid DetailId)
    : IDomainEvent;