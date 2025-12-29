namespace RenStore.Domain.Enums;

public enum ProductComplainStatus
{
    New,
    InReview,
    Resolved,      // жалоба обоснована → приняли меры
    Rejected
}