namespace RenStore.Identity.Domain.Enums;

public enum ApplicationUserStatus
{
    UnderReview, // после регистрации, до подтверждения email
    IsActive,
    Locked,
    IsDeleted
}