using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.Capture;

public sealed class CapturePaymentCommandValidator
    : AbstractValidator<CapturePaymentCommand>
{
    public CapturePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.ExternalPaymentId).NotEmpty().MaximumLength(256);
    }
}