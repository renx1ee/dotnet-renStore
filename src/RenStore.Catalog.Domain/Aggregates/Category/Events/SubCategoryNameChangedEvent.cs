using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Category.Events;

public sealed record SubCategoryNameChangedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid CategoryId,
    Guid SubCategoryId,
    string Name,
    string NormalizedName)
    : IDomainEvent;