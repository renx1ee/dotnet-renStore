using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiateRefund;

internal sealed class InitiateRefundCommandValidator
    : AbstractValidator<InitiateRefundCommand>
{
    public InitiateRefundCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}