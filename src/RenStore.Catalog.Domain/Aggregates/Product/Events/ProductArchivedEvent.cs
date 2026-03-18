using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

public sealed record ProductArchivedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    Guid ProductId,
    ProductStatus Status,
    DateTimeOffset OccurredAt)
    : IDomainEvent;