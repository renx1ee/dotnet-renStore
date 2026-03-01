using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class CatalogHomeItemReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Article { get; set; }
    public string StoragePath { get; set; }
    public int? Weight { get; set; }
    public int? Height { get; set; }
    public decimal? Amount { get; set; }
    public string Currency { get; set; }
}