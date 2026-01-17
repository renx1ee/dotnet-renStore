namespace RenStore.Domain.Entities;

public class SellerEntity
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset LastUpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public bool IsBlocked { get; set; } = false;
    public string Url { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
    /*public IEnumerable<ProductEntity>? Products { get; set; }*/
    public IEnumerable<SellerImageEntity>? SellerImages { get; set; }
    /*public IEnumerable<ProductAnswerEntity>? ProductAnswers { get; set; }*/
    public IEnumerable<SellerComplainEntity>? Complains { get; set; }
    /*public long SellerFooterImageId { get; set; }
    public SellerFooterImage? SellerFooterImage { get; set; }*/
}
