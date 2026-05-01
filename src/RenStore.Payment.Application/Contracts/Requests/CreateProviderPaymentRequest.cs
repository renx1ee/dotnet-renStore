namespace RenStore.Payment.Application.Contracts.Requests;

public sealed record CreateProviderPaymentRequest(
    Guid    PaymentId,
    decimal Amount,
    string  Currency,
    string  Description,
    string  ReturnUrl);