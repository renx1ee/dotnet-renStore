using RenStore.Payment.Domain.Enums;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

internal static class StatusConversion
{
    internal static string PaymentStatusToDatabase(PaymentStatus status)
    {
        return status switch
        {
            PaymentStatus.Pending           => "pending",
            PaymentStatus.Authorized        => "authorized",
            PaymentStatus.Captured          => "captured",
            PaymentStatus.Failed            => "failure",
            PaymentStatus.Cancelled         => "cancelled",
            PaymentStatus.Refunded          => "refunded",
            PaymentStatus.PartiallyRefunded => "partially-refunded",
            PaymentStatus.Expired           => "expired",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
    
    internal static PaymentStatus PaymentStatusFromDatabase(string status)
    {
        return status switch
        {
            "pending"            => PaymentStatus.Pending,
            "authorized"         => PaymentStatus.Authorized,
            "captured"           => PaymentStatus.Captured,
            "failure"            => PaymentStatus.Failed,
            "cancelled"          => PaymentStatus.Cancelled,
            "refunded"           => PaymentStatus.Refunded,
            "partially-refunded" => PaymentStatus.PartiallyRefunded,
            "expired"            => PaymentStatus.Expired,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
}