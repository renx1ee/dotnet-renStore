using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.Authorize;

public sealed class AuthorizePaymentCommandValidator
    : AbstractValidator<AuthorizePaymentCommand>
{
    public AuthorizePaymentCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.AttemptId).NotEmpty();
        RuleFor(x => x.ExternalPaymentId).NotEmpty().MaximumLength(256);
    }
}