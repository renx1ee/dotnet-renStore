namespace RenStore.Domain.Enums;

public enum ReviewComplainStatus
{
    New,
    InReview,
    Resolved,      // жалоба обоснована → приняли меры
    Rejected
}