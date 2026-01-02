using System.ComponentModel.DataAnnotations;

namespace RenStore.Domain.Enums;

public enum PromoCodeStatus
{
    Active = 0,
    Used = 1,
    Expired = 2,
    Disabled = 3,
    Scheduled = 4
}