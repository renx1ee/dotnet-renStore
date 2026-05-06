namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToPickupPoint;

public sealed class ShipToPickupPointCommandValidator
    : AbstractValidator<ShipToPickupPointCommand>
{
    public ShipToPickupPointCommandValidator()
    {
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
        RuleFor(x => x.PickupPointId).GreaterThan(0);
    }
}