using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.Domain.ReadModels;

public sealed class PaymentReadModel
{
    public Guid            Id                { get; set; }
    public Guid            OrderId           { get; set; }
    public Guid            CustomerId        { get; set; }
    public decimal         Amount            { get; set; }
    public decimal         RefundedAmount    { get; set; }
    public Currency        Currency          { get; set; }
    public PaymentStatus   Status            { get; set; }
    public PaymentProvider Provider          { get; set; }
    public PaymentMethod   PaymentMethod     { get; set; }
    public string?         ExternalPaymentId { get; set; }
    public string?         FailureReason     { get; set; }
    public DateTimeOffset  ExpiresAt         { get; set; }
    public DateTimeOffset? CapturedAt        { get; set; }
    public DateTimeOffset  CreatedAt         { get; set; }
    public DateTimeOffset? UpdatedAt         { get; set; }
}