namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAwaitingPickup;

public sealed class MarkAsAwaitingPickupCommandValidator
    : AbstractValidator<MarkAsAwaitingPickupCommand>
{
    public MarkAsAwaitingPickupCommandValidator()
    {
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
        RuleFor(x => x.PickupPointId).GreaterThan(0);
    }
}