using RenStore.Payment.Domain.Enums;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

internal static class RefundStatusConversion
{
    internal static RefundStatus RefundStatusFromDatabase(string status)
    {
        return status switch
        {
            "cancelled" => RefundStatus.Cancelled,
            "pending"   => RefundStatus.Pending,
            "failed"    => RefundStatus.Failed,
            "success"   => RefundStatus.Succeeded,
            _ => throw new InvalidOperationException(nameof(status))  
        };
    }
    
    internal static string RefundStatusToDatabase(RefundStatus status)
    {
        return status switch
        {
            RefundStatus.Cancelled => "cancelled",
            RefundStatus.Pending   => "pending",
            RefundStatus.Failed    => "failed",
            RefundStatus.Succeeded => "success",
            _ => throw new InvalidOperationException(nameof(status))  
        };
    }
}