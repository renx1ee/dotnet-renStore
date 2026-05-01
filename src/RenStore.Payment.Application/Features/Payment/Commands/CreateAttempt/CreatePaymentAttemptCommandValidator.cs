using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.CreateAttempt;

internal sealed class CreatePaymentAttemptCommandValidator
    : AbstractValidator<CreatePaymentAttemptCommand>
{
    public CreatePaymentAttemptCommandValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
    }
}