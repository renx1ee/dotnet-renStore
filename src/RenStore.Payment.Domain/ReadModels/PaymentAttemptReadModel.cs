namespace RenStore.Payment.Domain.ReadModels;

public sealed class PaymentAttemptReadModel
{
    public Guid            Id               { get; set; }
    public Guid            PaymentId        { get; set; }
    public int             AttemptNumber    { get; set; }
    public bool            IsSuccessful     { get; set; }
    public string?         FailureReason    { get; set; }
    public string?         ErrorCode        { get; set; }
    public string?         ExternalAuthCode { get; set; }
    public DateTimeOffset  CreatedAt        { get; set; }
    public DateTimeOffset? ResolvedAt       { get; set; }
}