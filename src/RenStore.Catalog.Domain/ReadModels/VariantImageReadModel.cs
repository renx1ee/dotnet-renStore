namespace RenStore.Catalog.Domain.ReadModels;

public sealed class VariantImageReadModel
{
    public Guid Id { get; init; }
    public string OriginalFileName { get; init; }
    public string StoragePath { get; init; }
    public long FileSizeBytes { get; init; }
    public bool IsMain { get; init; }
    public short SortOrder { get; init; } 
    public int Weight { get; init; }
    public int Height { get; init; }
    public bool IsDeleted { get; init; }
    public DateTimeOffset UploadedAt { get; init; } 
    public DateTimeOffset? UpdatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public Guid VariantId { get; init; }
}