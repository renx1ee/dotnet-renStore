namespace RenStore.Inventory.Application.Common;

public sealed class ReservationOptions
{
    public const string SectionName = "Reservation";
    public int TtlMinutes { get; set; } = 15;
    public TimeSpan Ttl => TimeSpan.FromMinutes(TtlMinutes);
}