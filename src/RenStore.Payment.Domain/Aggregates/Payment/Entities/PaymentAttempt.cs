namespace RenStore.Payment.Domain.Aggregates.Payment.Entities;

public sealed class PaymentAttempt
{
    public Guid           Id            { get; private set; }
    public Guid           PaymentId     { get; private set; }
    public int            AttemptNumber { get; private set; }
    public bool           IsSuccessful  { get; private set; }
    public string?        FailureReason { get; private set; }
    public string?        ErrorCode     { get; private set; }
    public string?        ExternalAuthCode { get; private set; }
    public DateTimeOffset CreatedAt     { get; private set; }
    public DateTimeOffset? ResolvedAt   { get; private set; }

    private PaymentAttempt() { }

    public static PaymentAttempt Create(
        Guid           attemptId,
        Guid           paymentId,
        int            attemptNumber,
        DateTimeOffset now)
    {
        return new PaymentAttempt
        {
            Id            = attemptId,
            PaymentId     = paymentId,
            AttemptNumber = attemptNumber,
            CreatedAt     = now
        };
    }

    internal void MarkAsSuccessful(
        DateTimeOffset now,
        string?        externalAuthCode)
    {
        IsSuccessful     = true;
        ExternalAuthCode = externalAuthCode;
        ResolvedAt       = now;
    }

    internal void MarkAsFailed(
        DateTimeOffset now,
        string         reason,
        string?        errorCode)
    {
        IsSuccessful  = false;
        FailureReason = reason;
        ErrorCode     = errorCode;
        ResolvedAt    = now;
    }
}