namespace RenStore.Payment.Application.Contracts.Requests;

public sealed record CreateProviderRefundResult(
    string ExternalRefundId,
    bool   IsSucceeded);