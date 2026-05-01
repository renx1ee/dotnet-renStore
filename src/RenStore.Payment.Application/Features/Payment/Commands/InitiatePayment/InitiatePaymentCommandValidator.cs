using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiatePayment;

internal sealed class InitiatePaymentCommandValidator
    : AbstractValidator<InitiatePaymentCommand>
{
    public InitiatePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(128);
    }
}