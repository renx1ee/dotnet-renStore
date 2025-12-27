namespace RenStore.Domain.Entities;

public class ProductAnswerEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ModeratedDate { get; set; } = null;
    public bool? IsApproved { get; set; } = null;
    public long SellerId { get; set; }
    public SellerEntity? Seller { get; set; }
    public Guid QuestionId { get; set; }
    public ProductQuestionEntity? Question { get; set; }
    public AnswerComplainEntity? Complain { get; set; }
    // public int ModerationStatusId { get; set; }
}