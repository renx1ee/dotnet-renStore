using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class ReviewComplainEntity
{
    public Guid Id { get; set; }
    public ReviewComplainReason Reason { get; set; }
    public string? CustomReason { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ReviewComplainStatus Status { get; set; } = ReviewComplainStatus.New;
    public DateTime? ResolvedAt { get; set; }
    public string ModeratorComment { get; set; } = string.Empty;
    // public ModeratorEntity? Moderator { get; set; }
    public Guid? ModeratorId { get; set; }
    public Guid ReviewId { get; set; }
    public ReviewEntity? Review { get; set; }
    public string UserId { get; set; } = string.Empty;
}