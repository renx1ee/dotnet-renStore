namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ArriveAtSortingCenter;

public sealed class ArrivedAtSortingCenterCommandValidator
    : AbstractValidator<ArrivedAtSortingCenterCommand>
{
    public ArrivedAtSortingCenterCommandValidator()
    {
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
        RuleFor(x => x.SortingCenterId).GreaterThan(0);
    }
}