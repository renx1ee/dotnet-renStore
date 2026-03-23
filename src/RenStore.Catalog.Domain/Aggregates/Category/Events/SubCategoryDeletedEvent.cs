using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Category.Events;

public sealed record SubCategoryDeletedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid SubCategoryId,
    Guid CategoryId)
    : IDomainEvent;