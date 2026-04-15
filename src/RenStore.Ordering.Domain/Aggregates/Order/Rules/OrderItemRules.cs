using RenStore.Order.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Domain.Aggregates.Order.Rules;

internal static class OrderItemRules
{
    public static void ValidateVariantId(Guid variantId)
    {
        if (variantId == Guid.Empty)
            throw new DomainException("Variant ID cannot be empty.");
    }

    public static void ValidateSizeId(Guid sizeId)
    {
        if (sizeId == Guid.Empty)
            throw new DomainException("Size ID cannot be empty.");
    }

    public static void ValidateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be greater than zero.");

        if (quantity > OrderConstants.OrderItem.MaxQuantity)
            throw new DomainException(
                $"Quantity cannot exceed {OrderConstants.OrderItem.MaxQuantity}.");
    }

    public static void ValidatePrice(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Price amount must be greater than zero.");
    }

    public static string ValidateAndTrimProductNameSnapshot(string productNameSnapshot)
    {
        if (string.IsNullOrWhiteSpace(productNameSnapshot))
            throw new DomainException("Product name snapshot cannot be null or whitespace.");

        var trimmed = productNameSnapshot.Trim();

        if (trimmed.Length > OrderConstants.OrderItem.ProductNameSnapshotMaxLength)
            throw new DomainException(
                $"Product name snapshot cannot exceed {OrderConstants.OrderItem.ProductNameSnapshotMaxLength} characters.");

        return trimmed;
    }
}