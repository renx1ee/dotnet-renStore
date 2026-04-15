using RenStore.Order.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Order.Domain.Aggregates.Order.Rules;

public static class OrderRules
{
    public static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("Customer ID cannot be empty.");
    }

    public static string ValidateAndTrimShippingAddress(string shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new DomainException("Shipping address cannot be null or whitespace.");

        var trimmed = shippingAddress.Trim();

        if (trimmed.Length < OrderConstants.Order.ShippingAddressMinLength)
            throw new DomainException(
                $"Shipping address must be at least {OrderConstants.Order.ShippingAddressMinLength} characters.");

        if (trimmed.Length > OrderConstants.Order.ShippingAddressMaxLength)
            throw new DomainException(
                $"Shipping address cannot exceed {OrderConstants.Order.ShippingAddressMaxLength} characters.");

        return trimmed;
    }

    public static string ValidateAndTrimReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Reason cannot be null or whitespace.");

        var trimmed = reason.Trim();

        if (trimmed.Length > 1000)
            throw new DomainException("Reason cannot exceed 1000 characters.");

        return trimmed;
    }
    
    public static string ValidateAndTrimTrackingNumber(string trackingNumber)
    {
        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new DomainException("Tracking number cannot be null or whitespace.");

        var trimmed = trackingNumber.Trim();

        if (trimmed.Length > 100)
            throw new DomainException("Tracking number cannot exceed 100 characters.");

        return trimmed;
    }
}