using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class AnswerComplainEntity
{
    public Guid Id { get; set; }
    public AnswerComplainReason Reason { get; set; }
    public string? CustomReason { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public AnswerComplainStatus Status { get; set; } = AnswerComplainStatus.New;
    public DateTime? ResolvedAt { get; set; }
    public string? ModeratorComment { get; set; } = string.Empty;
    // public ModeratorEntity? Moderator { get; set; }
    public Guid? ModeratorId { get; set; }
    public Guid ProductAnswerId { get; set; }
    /*public ProductAnswerEntity? ProductAnswer { get; set; }*/
    public string UserId { get; set; } = string.Empty;
    /*public ApplicationUser? User { get; set; }*/
}