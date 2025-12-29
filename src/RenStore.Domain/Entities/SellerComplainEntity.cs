using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class SellerComplainEntity
{
    public Guid Id { get; set; }
    public SellerComplainReason Reason { get; set; }
    public string? CustomReason { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public SellerComplainStatus Status { get; set; } = SellerComplainStatus.New;
    public DateTime? ResolvedAt { get; set; }
    public string ModeratorComment { get; set; } = string.Empty;
    public Guid? ModeratorId { get; set; }
    public long SellerId { get; set; }
    public SellerEntity? Seller { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    // public ModeratorEntity? Moderator { get; set; }
}