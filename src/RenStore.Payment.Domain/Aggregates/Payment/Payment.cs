// Domain/Aggregates/Payment/Payment.cs
using RenStore.Payment.Domain.Aggregates.Payment.Entities;
using RenStore.Payment.Domain.Aggregates.Payment.Events;
using RenStore.Payment.Domain.Aggregates.Payment.Rules;
using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Domain.Aggregates.Payment;

/// <summary>
/// Represents a payment lifecycle for a single order.
/// Supports authorization, capture, cancellation, expiration, and refunds.
/// </summary>
public sealed class Payment : AggregateRoot
{
    private readonly List<PaymentAttempt> _attempts = new();
    private readonly List<Refund>         _refunds  = new();

    /// <summary>Unique identifier of the payment.</summary>
    public Guid Id { get; private set; }

    /// <summary>Associated order identifier.</summary>
    public Guid OrderId { get; private set; }

    /// <summary>Customer who initiated the payment.</summary>
    public Guid CustomerId { get; private set; }

    /// <summary>Original payment amount.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Payment currency.</summary>
    public Currency Currency { get; private set; }

    /// <summary>Current lifecycle status.</summary>
    public PaymentStatus Status { get; private set; }

    /// <summary>Payment provider (YooKassa, Stripe, etc.).</summary>
    public PaymentProvider Provider { get; private set; }

    /// <summary>Payment method used by the customer.</summary>
    public PaymentMethod PaymentMethod { get; private set; }

    /// <summary>External transaction ID from the payment provider.</summary>
    public string? ExternalPaymentId { get; private set; }

    /// <summary>Reason for failure or cancellation.</summary>
    public string? FailureReason { get; private set; }

    /// <summary>Total amount already refunded.</summary>
    public decimal RefundedAmount { get; private set; }

    /// <summary>When the payment was captured (money charged).</summary>
    public DateTimeOffset? CapturedAt { get; private set; }

    /// <summary>When the payment expires if not completed.</summary>
    public DateTimeOffset ExpiresAt { get; private set; }

    /// <summary>When the payment was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>When the payment was last updated.</summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>All payment attempts (including failed ones).</summary>
    public IReadOnlyCollection<PaymentAttempt> Attempts => _attempts.AsReadOnly();

    /// <summary>All refunds associated with this payment.</summary>
    public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

    private Payment() { }

    public static Payment Create(
        DateTimeOffset  now,
        Guid            orderId,
        Guid            customerId,
        decimal         amount,
        Currency        currency,
        PaymentProvider provider,
        PaymentMethod   paymentMethod,
        TimeSpan?       expiresIn = null)
    {
        PaymentRules.ValidateOrderId(orderId);
        PaymentRules.ValidateCustomerId(customerId);
        PaymentRules.ValidateAmount(amount);

        var paymentId = Guid.NewGuid();
        var expiresAt = now.Add(expiresIn ?? TimeSpan.FromMinutes(30));

        var payment = new Payment();

        payment.Raise(new PaymentCreatedEvent(
            EventId:       Guid.NewGuid(),
            OccurredAt:    now,
            PaymentId:     paymentId,
            OrderId:       orderId,
            CustomerId:    customerId,
            Amount:        amount,
            Currency:      currency,
            Provider:      provider,
            PaymentMethod: paymentMethod,
            ExpiresAt:     expiresAt,
            Status:        PaymentStatus.Pending));

        return payment;
    }

    /// <summary>
    /// Creates a new payment attempt. Called before redirecting to provider.
    /// </summary>
    public Guid CreateAttempt(DateTimeOffset now)
    {
        EnsureInStatus(
            expected: PaymentStatus.Pending,
            message: "Cannot create attempt for payment not in Pending status.");

        EnsureNotExpired(now);

        var attemptId = Guid.NewGuid();
        var attemptNumber = _attempts.Count + 1;

        Raise(new PaymentAttemptCreatedEvent(
            EventId:       Guid.NewGuid(),
            OccurredAt:    now,
            PaymentId:     Id,
            AttemptId:     attemptId,
            AttemptNumber: attemptNumber));

        return attemptId;
    }

    /// <summary>
    /// Marks payment as authorized — provider confirmed and froze funds.
    /// </summary>
    public void Authorize(
        DateTimeOffset now,
        Guid           attemptId,
        string         externalPaymentId,
        string?        externalAuthCode = null)
    {
        EnsureInStatus(
            PaymentStatus.Pending,
            "Cannot authorize payment not in Pending status.");

        PaymentRules.ValidateExternalPaymentId(externalPaymentId);

        var attempt = GetAttempt(attemptId);

        Raise(new PaymentAuthorizedEvent(
            EventId:           Guid.NewGuid(),
            OccurredAt:        now,
            PaymentId:         Id,
            AttemptId:         attemptId,
            ExternalPaymentId: externalPaymentId,
            ExternalAuthCode:  externalAuthCode));
    }

    /// <summary>
    /// Captures the payment — money is actually charged.
    /// </summary>
    public void Capture(
        DateTimeOffset now,
        string         externalPaymentId)
    {
        EnsureInStatus(
            expected: PaymentStatus.Authorized,
            message:  "Cannot capture payment not in Authorized status.");

        PaymentRules.ValidateExternalPaymentId(externalPaymentId);

        Raise(new PaymentCapturedEvent(
            EventId:           Guid.NewGuid(),
            OccurredAt:        now,
            PaymentId:         Id,
            ExternalPaymentId: externalPaymentId));
    }

    /// <summary>
    /// Marks payment as failed after a provider error.
    /// </summary>
    public void Fail(
        DateTimeOffset now,
        Guid           attemptId,
        string         reason,
        string?        providerErrorCode = null)
    {
        if (Status != PaymentStatus.Pending && 
            Status != PaymentStatus.Authorized)
        {
            throw new DomainException(
                "Cannot fail payment not in Pending or Authorized status.");
        }

        PaymentRules.ValidateReason(reason);

        GetAttempt(attemptId);

        Raise(new PaymentFailedEvent(
            EventId:           Guid.NewGuid(),
            OccurredAt:        now,
            PaymentId:         Id,
            AttemptId:         attemptId,
            FailureReason:     reason,
            ProviderErrorCode: providerErrorCode));
    }

    /// <summary>
    /// Cancels the payment before capture.
    /// </summary>
    public void Cancel(
        DateTimeOffset now,
        string         reason)
    {
        if (Status != PaymentStatus.Pending && 
            Status != PaymentStatus.Authorized)
        {
            throw new DomainException(
                "Cannot cancel payment not in Pending or Authorized status.");
        }

        PaymentRules.ValidateReason(reason);

        Raise(new PaymentCancelledEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            PaymentId:  Id,
            Reason:     reason));
    }

    /// <summary>
    /// Expires the payment if customer did not complete it in time.
    /// </summary>
    public void Expire(DateTimeOffset now)
    {
        if (Status != PaymentStatus.Pending)
            throw new DomainException(
                "Cannot expire payment not in Pending status.");

        if (now < ExpiresAt)
            throw new DomainException(
                "Payment has not expired yet.");

        Raise(new PaymentExpiredEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            PaymentId:  Id));
    }

    /// <summary>
    /// Initiates a refund. Supports both full and partial refunds.
    /// </summary>
    public Guid InitiateRefund(
        DateTimeOffset now,
        decimal        amount,
        string         reason)
    {
        if (Status != PaymentStatus.Captured &&
            Status != PaymentStatus.PartiallyRefunded)
            throw new DomainException(
                "Cannot refund payment not in Captured or PartiallyRefunded status.");

        PaymentRules.ValidateRefundAmount(amount, Amount, RefundedAmount);
        PaymentRules.ValidateReason(reason);

        var refundId = Guid.NewGuid();

        Raise(new RefundInitiatedEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            PaymentId:  Id,
            RefundId:   refundId,
            Amount:     amount,
            Currency:   Currency,
            Reason:     reason));

        return refundId;
    }

    /// <summary>
    /// Marks a refund as succeeded after provider confirmation.
    /// </summary>
    public void SucceedRefund(
        DateTimeOffset now,
        Guid           refundId,
        string         externalRefundId)
    {
        var refund = GetRefund(refundId);

        if (refund.Status != RefundStatus.Pending)
            throw new DomainException(
                "Cannot succeed refund not in Pending status.");

        PaymentRules.ValidateExternalPaymentId(externalRefundId);

        Raise(new RefundSucceededEvent(
            EventId:         Guid.NewGuid(),
            OccurredAt:      now,
            PaymentId:       Id,
            RefundId:        refundId,
            ExternalRefundId: externalRefundId));
    }

    /// <summary>
    /// Marks a refund as failed.
    /// </summary>
    public void FailRefund(
        DateTimeOffset now,
        Guid           refundId,
        string         reason)
    {
        var refund = GetRefund(refundId);

        if (refund.Status != RefundStatus.Pending)
            throw new DomainException(
                "Cannot fail refund not in Pending status.");

        PaymentRules.ValidateReason(reason);

        Raise(new RefundFailedEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            PaymentId:  Id,
            RefundId:   refundId,
            Reason:     reason));
    }

    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case PaymentCreatedEvent e:
                Id            = e.PaymentId;
                OrderId       = e.OrderId;
                CustomerId    = e.CustomerId;
                Amount        = e.Amount;
                Currency      = e.Currency;
                Provider      = e.Provider;
                PaymentMethod = e.PaymentMethod;
                Status        = e.Status;
                ExpiresAt     = e.ExpiresAt;
                CreatedAt     = e.OccurredAt;
                break;

            case PaymentAttemptCreatedEvent e:
                _attempts.Add(PaymentAttempt.Create(
                    attemptId:     e.AttemptId,
                    paymentId:     e.PaymentId,
                    attemptNumber: e.AttemptNumber,
                    now:           e.OccurredAt));
                UpdatedAt = e.OccurredAt;
                break;

            case PaymentAuthorizedEvent e:
                var authorizedAttempt = GetAttempt(e.AttemptId);
                authorizedAttempt.MarkAsSuccessful(e.OccurredAt, e.ExternalAuthCode);
                ExternalPaymentId = e.ExternalPaymentId;
                Status            = PaymentStatus.Authorized;
                UpdatedAt         = e.OccurredAt;
                break;

            case PaymentCapturedEvent e:
                Status     = PaymentStatus.Captured;
                CapturedAt = e.OccurredAt;
                UpdatedAt  = e.OccurredAt;
                break;

            case PaymentFailedEvent e:
                var failedAttempt = GetAttempt(e.AttemptId);
                failedAttempt.MarkAsFailed(e.OccurredAt, e.FailureReason, e.ProviderErrorCode);
                Status        = PaymentStatus.Failed;
                FailureReason = e.FailureReason;
                UpdatedAt     = e.OccurredAt;
                break;

            case PaymentCancelledEvent e:
                Status        = PaymentStatus.Cancelled;
                FailureReason = e.Reason;
                UpdatedAt     = e.OccurredAt;
                break;

            case PaymentExpiredEvent e:
                Status    = PaymentStatus.Expired;
                UpdatedAt = e.OccurredAt;
                break;

            case RefundInitiatedEvent e:
                _refunds.Add(Refund.Create(
                    refundId:  e.RefundId,
                    paymentId: e.PaymentId,
                    amount:    e.Amount,
                    currency:  e.Currency,
                    reason:    e.Reason,
                    now:       e.OccurredAt));
                UpdatedAt = e.OccurredAt;
                break;

            case RefundSucceededEvent e:
                var succeededRefund = GetRefund(e.RefundId);
                succeededRefund.MarkAsSucceeded(e.OccurredAt, e.ExternalRefundId);
                RefundedAmount += succeededRefund.Amount;
                Status = RefundedAmount >= Amount
                    ? PaymentStatus.Refunded
                    : PaymentStatus.PartiallyRefunded;
                UpdatedAt = e.OccurredAt;
                break;

            case RefundFailedEvent e:
                var failedRefund = GetRefund(e.RefundId);
                failedRefund.MarkAsFailed(e.OccurredAt, e.Reason);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }

    public static Payment Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var payment = new Payment();

        foreach (var @event in history)
        {
            payment.Apply(@event);
            payment.Version++;
        }

        return payment;
    }

    private void EnsureInStatus(PaymentStatus expected, string message)
    {
        if (Status != expected)
            throw new DomainException(message);
    }

    private void EnsureNotExpired(DateTimeOffset now)
    {
        if (now >= ExpiresAt)
            throw new DomainException(
                "Payment has expired and cannot be processed.");
    }

    private PaymentAttempt GetAttempt(Guid attemptId)
    {
        return _attempts.FirstOrDefault(x => x.Id == attemptId)
            ?? throw new DomainException($"Payment attempt {attemptId} not found.");
    }

    private Refund GetRefund(Guid refundId)
    {
        return _refunds.FirstOrDefault(x => x.Id == refundId)
            ?? throw new DomainException($"Refund {refundId} not found.");
    }
}