using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class PriceHistoryReadModel
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public DateTimeOffset ValidFrom { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? DeactivatedAt { get; set; }
    public Guid SizeId { get; set; }
}