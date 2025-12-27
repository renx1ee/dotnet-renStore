using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class QuestionComplainEntity
{
    public Guid Id { get; set; }
    public QuestionComplainReason Reason { get; set; }
    public string? CustomReason { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public QuestionComplainStatus Status { get; set; } = QuestionComplainStatus.New;
    public DateTime? ResolvedAt { get; set; }
    public string ModeratorComment { get; set; } = string.Empty;
    // public ModeratorEntity? Moderator { get; set; }
    public Guid? ModeratorId { get; set; }
    public Guid ProductQuestionId { get; set; }
    public ProductQuestionEntity? ProductQuestion { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
}