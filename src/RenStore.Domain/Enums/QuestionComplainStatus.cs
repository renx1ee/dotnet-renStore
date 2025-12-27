namespace RenStore.Domain.Enums;

public enum QuestionComplainStatus
{
    New,
    InReview,
    Resolved,      // жалоба обоснована → приняли меры
    Rejected
}