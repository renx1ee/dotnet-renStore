using Microsoft.EntityFrameworkCore;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Persistence.Write.Projections;

internal sealed class PaymentAttemptProjection : IPaymentAttemptProjection
{
    private readonly PaymentDbContext _context;

    public PaymentAttemptProjection(PaymentDbContext context)
    {
        _context = context
            ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(
        PaymentAttemptReadModel attempt,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(attempt);

        await _context.PaymentAttempts.AddAsync(attempt, cancellationToken);
    }

    public async Task MarkAsSuccessfulAsync(
        DateTimeOffset now,
        Guid           attemptId,
        string?        externalAuthCode,
        CancellationToken cancellationToken)
    {
        var attempt = await GetAttemptAsync(attemptId, cancellationToken);

        attempt.IsSuccessful     = true;
        attempt.ExternalAuthCode = externalAuthCode;
        attempt.ResolvedAt       = now;
    }

    public async Task MarkAsFailedAsync(
        DateTimeOffset now,
        Guid           attemptId,
        string         failureReason,
        string?        errorCode,
        CancellationToken cancellationToken)
    {
        var attempt = await GetAttemptAsync(attemptId, cancellationToken);

        attempt.IsSuccessful  = false;
        attempt.FailureReason = failureReason;
        attempt.ErrorCode     = errorCode;
        attempt.ResolvedAt    = now;
    }

    private async Task<PaymentAttemptReadModel> GetAttemptAsync(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var attempt = await _context.PaymentAttempts
            .FirstOrDefaultAsync(
                x => x.Id == attemptId,
                cancellationToken);

        return attempt
            ?? throw new NotFoundException(typeof(PaymentAttemptReadModel), attemptId);
    }
}