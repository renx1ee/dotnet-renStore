namespace RenStore.Domain.Enums;

public enum AnswerComplainStatus
{
    New,
    InReview,
    Resolved,      // жалоба обоснована → приняли меры
    Rejected
}