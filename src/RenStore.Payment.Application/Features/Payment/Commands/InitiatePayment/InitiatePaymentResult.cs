namespace RenStore.Payment.Application.Features.Payment.Commands.InitiatePayment;

public sealed record InitiatePaymentResult(
    Guid   PaymentId,
    Guid   AttemptId,
    string ConfirmationUrl);