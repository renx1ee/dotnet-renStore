using RenStore.Domain.Enums;

namespace RenStore.Domain.Entities;

public class ReviewEntity
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal ReviewRating { get; set; } = 0;
    public DateTime CreatedDate = DateTime.UtcNow;
    public DateTime? LastUpdatedDate = null;
    public bool IsUpdated = false;
    public DateTime? ModeratedDate = null;
    public ReviewStatus Status { get; set; }
    public bool? IsApproved = null;
    public ApplicationUser? ApplicationUser { get; set; }
    public string UserId { get; set; }
    /*public ProductVariant? ProductVariant { get; set; }*/
    public Guid ProductVariantId { get; set; }
    public IEnumerable<ReviewComplainEntity>? Complains { get; set; }
}