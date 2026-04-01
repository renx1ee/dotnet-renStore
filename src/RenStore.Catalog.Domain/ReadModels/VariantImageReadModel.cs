namespace RenStore.Catalog.Domain.ReadModels;

public sealed class VariantImageReadModel
{
    public Guid Id { get; set; }
    public string OriginalFileName { get; set; }
    public string StoragePath { get; set; }
    public long FileSizeBytes { get; set; }
    public bool IsMain { get; set; }
    public int SortOrder { get; set; } 
    public int Weight { get; set; }
    public int Height { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? UpdatedById { get; set; } 
    public string? UpdatedByRole { get; set; }  
    public DateTimeOffset UploadedAt { get; set; } 
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid VariantId { get; set; }
}