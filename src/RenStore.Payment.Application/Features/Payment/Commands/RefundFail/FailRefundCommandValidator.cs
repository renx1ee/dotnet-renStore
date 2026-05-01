using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundFail;

internal sealed class FailRefundCommandValidator
    : AbstractValidator<FailRefundCommand>
{
    public FailRefundCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.RefundId).NotEmpty();
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}