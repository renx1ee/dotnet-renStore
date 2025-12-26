namespace RenStore.Domain.Entities;

public class UserImageEntity
{
    public Guid Id { get; set; }
    public string OriginalFileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public bool IsMain { get; set; } = false;
    public short SortOrder { get; set; } = 0;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public int Weight { get; set; }
    public int Height { get; set; }
    public string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}