using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.Domain.Aggregates.Payment.Entities;

public sealed class Refund
{
    public Guid            Id               { get; private set; }
    public Guid            PaymentId        { get; private set; }
    public decimal         Amount           { get; private set; }
    public Currency        Currency         { get; private set; }
    public string          Reason           { get; private set; } = null!;
    public RefundStatus    Status           { get; private set; }
    public string?         ExternalRefundId { get; private set; }
    public string?         FailureReason    { get; private set; }
    public DateTimeOffset  CreatedAt        { get; private set; }
    public DateTimeOffset? ResolvedAt      { get; private set; }

    private Refund() { }

    public static Refund Create(
        Guid           refundId,
        Guid           paymentId,
        decimal        amount,
        Currency       currency,
        string         reason,
        DateTimeOffset now)
    {
        return new Refund
        {
            Id        = refundId,
            PaymentId = paymentId,
            Amount    = amount,
            Currency  = currency,
            Reason    = reason,
            Status    = RefundStatus.Pending,
            CreatedAt = now
        };
    }

    internal void MarkAsSucceeded(
        DateTimeOffset now,
        string         externalRefundId)
    {
        Status           = RefundStatus.Succeeded;
        ExternalRefundId = externalRefundId;
        ResolvedAt       = now;
    }

    internal void MarkAsFailed(
        DateTimeOffset now,
        string         reason)
    {
        Status        = RefundStatus.Failed;
        FailureReason = reason;
        ResolvedAt    = now;
    }
}