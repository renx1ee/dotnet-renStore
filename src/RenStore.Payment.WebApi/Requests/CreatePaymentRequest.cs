using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.WebApi.Requests;

public sealed record CreatePaymentRequest(
    Guid            OrderId,
    Guid            CustomerId,
    decimal         Amount,
    Currency        Currency,
    PaymentProvider Provider,
    PaymentMethod   PaymentMethod);