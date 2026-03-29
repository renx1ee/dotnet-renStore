namespace RenStore.Inventory.Domain.Constants;

public static class InventoryConstants
{
    public static class VariantStock
    {
        public const int MaxInventoryStockCount = 999999;
        public const int MinInventoryStockCount = 0;
    }
    
    public static class VariantReservation
    {
        public const int MaxInventoryReservationCount = 999999;
        public const int MinInventoryReservationCount = 0;
    }
}