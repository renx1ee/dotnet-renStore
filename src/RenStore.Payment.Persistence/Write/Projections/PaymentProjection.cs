using Microsoft.EntityFrameworkCore;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Persistence.Write.Projections;

internal sealed class PaymentProjection : IPaymentProjection
{
    private readonly PaymentDbContext _context;

    public PaymentProjection(PaymentDbContext context)
    {
        _context = context
            ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task<Guid> AddAsync(
        PaymentReadModel payment,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payment);

        await _context.Payments.AddAsync(payment, cancellationToken);

        return payment.Id;
    }

    public async Task SetAuthorizedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         externalPaymentId,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.Status            = PaymentStatus.Authorized;
        payment.ExternalPaymentId = externalPaymentId;
        payment.UpdatedAt         = now;
    }

    public async Task SetCapturedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.Status     = PaymentStatus.Captured;
        payment.CapturedAt = now;
        payment.UpdatedAt  = now;
    }

    public async Task SetFailedAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         failureReason,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.Status        = PaymentStatus.Failed;
        payment.FailureReason = failureReason;
        payment.UpdatedAt     = now;
    }

    public async Task SetCancelledAsync(
        DateTimeOffset now,
        Guid           paymentId,
        string         reason,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.Status        = PaymentStatus.Cancelled;
        payment.FailureReason = reason;
        payment.UpdatedAt     = now;
    }

    public async Task SetExpiredAsync(
        DateTimeOffset now,
        Guid           paymentId,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.Status    = PaymentStatus.Expired;
        payment.UpdatedAt = now;
    }

    public async Task UpdateRefundedAmountAsync(
        DateTimeOffset now,
        Guid           paymentId,
        decimal        refundedAmount,
        PaymentStatus  status,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);

        payment.RefundedAmount = refundedAmount;
        payment.Status         = status;
        payment.UpdatedAt      = now;
    }

    private async Task<PaymentReadModel> GetPaymentAsync(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(
                x => x.Id == paymentId,
                cancellationToken);

        return payment
            ?? throw new NotFoundException(typeof(PaymentReadModel), paymentId);
    }
    
    public async Task SetLastAttemptIdAsync(
        DateTimeOffset now,
        Guid           paymentId,
        Guid           lastAttemptId,
        CancellationToken cancellationToken)
    {
        var payment = await GetPaymentAsync(paymentId, cancellationToken);
        payment.LastAttemptId = lastAttemptId;
        payment.UpdatedAt     = now;
    }
}