using RenStore.Catalog.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

internal static class PriceHistoryRules
{
    internal static void ValidateSizeId(Guid sizeId)
    {
        if (sizeId == Guid.Empty)
            throw new DomainException(
                "Size ID cannot be Guid empty.");
    }

    internal static void ValidatePrice(decimal price)
    {
        if (price is < CatalogConstants.Price.MinPrice or > CatalogConstants.Price.MaxPrice)
            throw new DomainException(
                $"The price must be between {CatalogConstants.Price.MinPrice} and {CatalogConstants.Price.MaxPrice}.");
    }
}