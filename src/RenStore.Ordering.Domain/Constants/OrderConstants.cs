namespace RenStore.Order.Domain.Constants;

public static class OrderConstants
{
    public class Order
    {
        public const int ShippingAddressMinLength = 10;
        public const int ShippingAddressMaxLength = 500;
    }

    public class OrderItem
    {
        public const int MaxQuantity = 100;
        public const int ProductNameSnapshotMaxLength = 500;
    }
}