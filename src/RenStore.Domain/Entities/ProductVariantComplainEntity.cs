using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class ProductVariantComplainEntity
{
    public Guid Id { get; set; }
    public ProductComplainReason Reason { get; set; }
    public string? CustomReason { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ProductComplainStatus Status { get; set; } = ProductComplainStatus.New;
    public DateTime? ResolvedAt { get; set; }
    public string ModeratorComment { get; set; } = string.Empty;
    // public ModeratorEntity? Moderator { get; set; }
    public Guid? ModeratorId { get; set; }
    public Guid ProductVariantId { get; set; }
    public ProductVariantEntity? ProductVariant { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
}