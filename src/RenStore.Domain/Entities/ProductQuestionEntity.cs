/*namespace RenStore.Domain.Entities;

public class ProductQuestionEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    public DateTime ModeratedDate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
    public bool? IsApproved { get; set; } = null;
    public Guid ProductVariantId { get; set; }
    public ProductVariantEntity? ProductVariant { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }
    public ProductAnswerEntity? Answer { get; set; }
    public Guid AnswerId { get; set; }
    // public int ModerationStatusId { get; set; }
}*/