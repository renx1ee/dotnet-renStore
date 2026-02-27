using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class CatalogHomeItemReadModel
{
    // Variant
    public Guid Id { get; init; }
    public string Name { get; init; }
    public long Article { get; init; }
    // Image
    public string StoragePath { get; init; }
    public int Weight { get; init; }
    public int Height { get; init; }
    // Price
    public decimal Amount { get; init; }
    public Currency Currency { get; init; }
}