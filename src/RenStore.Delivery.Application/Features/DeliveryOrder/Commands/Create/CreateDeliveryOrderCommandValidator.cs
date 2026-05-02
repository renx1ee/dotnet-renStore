using FluentValidation;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Create;

public sealed class CreateDeliveryOrderCommandValidator
    : AbstractValidator<CreateDeliveryOrderCommand>
{
    public CreateDeliveryOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.DeliveryTariffId).GreaterThan(0);
    }
}