namespace RenStore.Catalog.Domain.Entities;

public class ProductImageEntity
{
    public Guid Id { get; private set; }
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoragePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public bool IsMain { get; private set; } = false;
    public short SortOrder { get; private set; } = 0;
    public DateTime UploadedAt { get; private set; } = DateTime.UtcNow;
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public ProductVariantEntity? ProductVariant { get; private set; }
}