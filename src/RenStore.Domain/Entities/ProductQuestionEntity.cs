using RenStore.Domain.Entities;

namespace RenStore.Catalog.Domain.Entities;

public class ProductQuestionEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModeratedDate { get; set; } = null;
    public bool? IsApproved { get; set; } = null;
    public Guid ProductVariantId { get; set; }
    public ProductVariant? ProductVariant { get; set; }
    public string UserId { get; set; }
    /*public ApplicationUser? User { get; set; }*/
    public Guid AnswerId { get; set; }
    /*public ProductAnswerEntity? Answer { get; set; }*/
    // public int ModerationStatusId { get; set; }
    public Guid ComplainId { get; set; }
    /*public QuestionComplainEntity? Complain { get; set; }*/
}