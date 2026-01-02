using System.ComponentModel.DataAnnotations;

namespace RenStore.Domain.Enums;

public enum PromoCodeType
{
    Percentage = 0,
    FixedAmount = 1,
    FreeShipping = 2,
    GiftCard = 3,
    FreeProduct = 4,
    NextOrderDiscount = 5,
    BonusPoints = 6
}