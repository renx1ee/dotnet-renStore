using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

internal static class PriceHistoryRules
{
    private const decimal MinPrice = 100;
    private const decimal MaxPrice = 1000000;
    
    internal static void ValidateSizeId(Guid sizeId)
    {
        if (sizeId == Guid.Empty)
            throw new DomainException(
                "Size ID cannot be Guid empty.");
    }

    internal static void ValidatePrice(decimal price)
    {
        if (price is < MinPrice or > MaxPrice)
            throw new DomainException(
                $"The price must be between {MinPrice} and {MaxPrice}.");
    }
}