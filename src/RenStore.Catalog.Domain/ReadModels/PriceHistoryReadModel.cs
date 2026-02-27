using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class PriceHistoryReadModel
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
    public DateTimeOffset ValidFrom { get; init; }
    public bool IsActive { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeactivatedAt { get; init; }
    public Guid SizeId { get; init; }
}