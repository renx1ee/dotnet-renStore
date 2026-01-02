using System.ComponentModel.DataAnnotations;

namespace RenStore.Domain.Enums;

public enum PromoCodeTarget
{
    All = 0,
    NewUsers = 1,
    LoyalCustomers = 2,
    SpecificUser = 3,
    Seller = 4,
    Category = 5,
    Product = 6,
    UserGroup = 7
}