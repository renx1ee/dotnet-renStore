namespace RenStore.Delivery.Application.Features.DeliveryOrder.Commands.RegisterWithRussianPost;

internal sealed class RegisterWithRussianPostCommandValidator
    : AbstractValidator<RegisterWithRussianPostCommand>
{
    public RegisterWithRussianPostCommandValidator()
    {
        RuleFor(x => x.DeliveryOrderId).NotEmpty();
        RuleFor(x => x.RecipientName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.RecipientPhone).NotEmpty().MaximumLength(20);
        RuleFor(x => x.AddressTo).NotEmpty().MaximumLength(500);
        RuleFor(x => x.PostcodeFrom).NotEmpty().Length(6)
            .Matches(@"^\d{6}$");
        RuleFor(x => x.PostcodeTo).NotEmpty().Length(6)
            .Matches(@"^\d{6}$");
        RuleFor(x => x.WeightGrams).GreaterThan(0);
        RuleFor(x => x.ValueRub).GreaterThanOrEqualTo(0);
    }
}