namespace RenStore.Payment.Application.Contracts.Requests;

public sealed record CreateProviderRefundRequest(
    string  ExternalPaymentId,
    decimal Amount,
    string  Currency,
    string  Reason);