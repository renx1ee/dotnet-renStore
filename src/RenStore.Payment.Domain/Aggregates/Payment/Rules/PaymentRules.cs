using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Domain.Aggregates.Payment.Rules;

internal static class PaymentRules
{
    public static void ValidateOrderId(Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("OrderId cannot be empty.");
    }

    public static void ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("CustomerId cannot be empty.");
    }

    public static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Payment amount must be greater than zero.");
    }

    public static void ValidateExternalPaymentId(string externalPaymentId)
    {
        if (string.IsNullOrWhiteSpace(externalPaymentId))
            throw new DomainException("ExternalPaymentId cannot be empty.");
    }

    public static void ValidateRefundAmount(decimal refundAmount, decimal originalAmount, decimal alreadyRefunded)
    {
        if (refundAmount <= 0)
            throw new DomainException("Refund amount must be greater than zero.");

        if (refundAmount > originalAmount - alreadyRefunded)
            throw new DomainException(
                $"Refund amount {refundAmount} exceeds remaining refundable amount {originalAmount - alreadyRefunded}.");
    }

    public static void ValidateReason(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("Reason cannot be empty.");

        if (reason.Length > 500)
            throw new DomainException("Reason cannot exceed 500 characters.");
    }
}