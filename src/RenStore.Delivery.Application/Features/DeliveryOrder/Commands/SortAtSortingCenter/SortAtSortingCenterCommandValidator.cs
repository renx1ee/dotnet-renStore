namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.SortAtSortingCenter;

internal sealed class SortAtSortingCenterCommandValidator
    : AbstractValidator<SortAtSortingCenterCommand>
{
    public SortAtSortingCenterCommandValidator()
    {
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
        RuleFor(x => x.SortingCenterId).GreaterThan(0);
    }
}