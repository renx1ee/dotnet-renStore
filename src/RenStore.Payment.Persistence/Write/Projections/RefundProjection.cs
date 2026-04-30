// Persistence/Write/Projections/RefundProjection.cs
using Microsoft.EntityFrameworkCore;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Persistence.Write.Projections;

internal sealed class RefundProjection : IRefundProjection
{
    private readonly PaymentDbContext _context;

    public RefundProjection(PaymentDbContext context)
    {
        _context = context
            ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(
        RefundReadModel refund,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(refund);

        await _context.Refunds.AddAsync(refund, cancellationToken);
    }

    public async Task MarkAsSucceededAsync(
        DateTimeOffset now,
        Guid           refundId,
        string         externalRefundId,
        CancellationToken cancellationToken)
    {
        var refund = await GetRefundAsync(refundId, cancellationToken);

        refund.Status           = RefundStatus.Succeeded;
        refund.ExternalRefundId = externalRefundId;
        refund.ResolvedAt       = now;
    }

    public async Task MarkAsFailedAsync(
        DateTimeOffset now,
        Guid           refundId,
        string         reason,
        CancellationToken cancellationToken)
    {
        var refund = await GetRefundAsync(refundId, cancellationToken);

        refund.Status        = RefundStatus.Failed;
        refund.FailureReason = reason;
        refund.ResolvedAt    = now;
    }

    private async Task<RefundReadModel> GetRefundAsync(
        Guid refundId,
        CancellationToken cancellationToken)
    {
        var refund = await _context.Refunds
            .FirstOrDefaultAsync(
                x => x.Id == refundId,
                cancellationToken);

        return refund
            ?? throw new NotFoundException(typeof(RefundReadModel), refundId);
    }
}