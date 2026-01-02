namespace RenStore.Domain.Entities;

public class PromoCodeUserLimit
{
    public Guid Id { get; set; }
    public PromoCodeEntity? PromoCode { get; set; }
    public Guid PromoCodeId { get; set; }
    public ApplicationUser? User { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int UsageCount { get; set; } = 0;
    public DateTime? LastUsedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}