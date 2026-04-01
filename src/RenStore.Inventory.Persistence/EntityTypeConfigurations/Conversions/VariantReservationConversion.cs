using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations.Conversions;

internal static class VariantReservationConversion
{
    internal static string ReservationStatusToDatabase(ReservationStatus status)
    {
        return status switch
        {
            ReservationStatus.Active => "active",
            ReservationStatus.Cancelled => "cancelled",
            ReservationStatus.Confirmed => "confirmed",
            ReservationStatus.Deleted => "deleted",
            ReservationStatus.Expired => "expired",
            ReservationStatus.Released => "released",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }
    
    internal static ReservationStatus ReservationStatusFromDatabase(string  status)
    {
        return status switch
        {
            "active" => ReservationStatus.Active,
            "cancelled" => ReservationStatus.Cancelled,
            "confirmed" => ReservationStatus.Confirmed,
            "deleted" => ReservationStatus.Deleted,
            "expired" => ReservationStatus.Expired,
            "released" => ReservationStatus.Released,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
    }

    internal static string ReservationCancelReasonToDatabase(ReservationCancelReason? reason)
    {
        return reason switch
        {   
            _ => throw new ArgumentOutOfRangeException(nameof(reason))
        };
    }
    
    internal static ReservationCancelReason ReservationCancelReasonFromDatabase(string? reason)
    {
        return reason switch
        {   
            _ => throw new ArgumentOutOfRangeException(nameof(reason))
        };
    }
}