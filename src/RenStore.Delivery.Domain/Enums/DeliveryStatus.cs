namespace RenStore.Delivery.Domain.Enums;

public enum DeliveryStatus
{
    AwaitingConfirmation,
    OnAssemblyByTheSeller,
    OnTheWayToTheSortingCenter,
    OnTheWay,
    OnTheWayToThePickUpPoint,
    AwaitingReceipt
}