namespace RenStore.Domain.Enums;

public enum SellerComplainStatus
{
    New,
    InReview,
    Resolved,      // жалоба обоснована → приняли меры
    Rejected
}