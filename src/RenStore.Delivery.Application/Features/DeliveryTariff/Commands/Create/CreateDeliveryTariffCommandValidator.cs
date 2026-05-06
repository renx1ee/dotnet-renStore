namespace RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Create;

public sealed class CreateDeliveryTariffCommandValidator
    : AbstractValidator<CreateDeliveryTariffCommand>
{
    public CreateDeliveryTariffCommandValidator()
    {
        RuleFor(x => x.PriceAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Currency).NotEmpty().Length(3);
        RuleFor(x => x.WeightLimitKg).GreaterThan(0);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.Description).MaximumLength(500);
    }
}