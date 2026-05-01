namespace RenStore.Payment.WebApi.Requests;

public sealed record InitiateRefundRequest(
    decimal Amount,
    string  Reason);