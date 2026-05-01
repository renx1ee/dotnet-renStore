using FluentValidation;

namespace RenStore.Payment.Application.Features.Payment.Commands.Create;

internal sealed class CreatePaymentCommandValidator
    : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required.");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.")
            .LessThanOrEqualTo(1_000_000)
            .WithMessage("Amount cannot exceed 1,000,000.");

        RuleFor(x => x.Currency)
            .IsInEnum()
            .WithMessage("Invalid currency.");

        RuleFor(x => x.Provider)
            .IsInEnum()
            .WithMessage("Invalid payment provider.");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum()
            .WithMessage("Invalid payment method.");
    }
}