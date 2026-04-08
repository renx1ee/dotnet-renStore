namespace RenStore.Catalog.Domain.ReadModels;

public sealed class CatalogReadModel
{
    // Variant
    public Guid VariantId { get; init; }
    public long Article { get; init; }
    public string VariantUrlSlug { get; init; }
    public string Name { get; init; }
    public DateTimeOffset CreatedDate { get; init; }
    // Image
    public string StoragePath { get; init; }
    public Guid ImageId { get; init; }
    public Guid ImageUrlSlug { get; init; }
    // Price
    public decimal? Amount { get; init; }
    public string Currency { get; init; }
}