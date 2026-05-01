using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.RefundSuccess;

internal sealed class SucceedRefundCommandValidator
    : AbstractValidator<SucceedRefundCommand>
{
    public SucceedRefundCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.RefundId).NotEmpty();
        RuleFor(x => x.ExternalRefundId).NotEmpty().MaximumLength(256);
    }
}