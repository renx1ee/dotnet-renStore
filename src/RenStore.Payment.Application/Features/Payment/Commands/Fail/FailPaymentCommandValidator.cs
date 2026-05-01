using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.Fail;

internal sealed class FailPaymentCommandValidator
    : AbstractValidator<FailPaymentCommand>
{
    public FailPaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.AttemptId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}