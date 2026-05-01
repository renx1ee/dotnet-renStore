namespace RenStore.Payment.Application.Contracts.Requests;

public sealed record CreateProviderPaymentResult(
    string  ExternalPaymentId,
    string  ConfirmationUrl,   // URL куда редиректить покупателя
    bool    IsSucceeded);