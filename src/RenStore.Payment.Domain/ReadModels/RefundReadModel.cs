using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.Domain.ReadModels;

public sealed class RefundReadModel
{
    public Guid            Id               { get; set; }
    public Guid            PaymentId        { get; set; }
    public decimal         Amount           { get; set; }
    public Currency        Currency         { get; set; }
    public string          Reason           { get; set; } = null!;
    public RefundStatus    Status           { get; set; }
    public string?         ExternalRefundId { get; set; }
    public string?         FailureReason    { get; set; }
    public DateTimeOffset  CreatedAt        { get; set; }
    public DateTimeOffset? ResolvedAt       { get; set; }
}