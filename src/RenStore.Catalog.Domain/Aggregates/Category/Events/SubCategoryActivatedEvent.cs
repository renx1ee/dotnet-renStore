using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Category.Events;

public sealed record SubCategoryActivatedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid CategoryId,
    Guid SubCategoryId)
    : IDomainEvent;