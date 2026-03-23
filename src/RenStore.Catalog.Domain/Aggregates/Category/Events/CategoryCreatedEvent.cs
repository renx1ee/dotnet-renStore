using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Category.Events;

public sealed record CategoryCreatedEvent(
    Guid UpdatedById,
    string UpdatedByRole,
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid CategoryId,
    bool IsActive,
    string Name,
    string NormalizedName,
    string NameRu,
    string NormalizedNameRu,
    string? Description = null)
    : IDomainEvent;