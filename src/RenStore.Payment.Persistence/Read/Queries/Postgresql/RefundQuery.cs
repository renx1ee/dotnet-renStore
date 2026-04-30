using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.Read.Base;

namespace RenStore.Payment.Persistence.Read.Queries.Postgresql;

internal sealed class RefundQuery(
    PaymentDbContext context, 
    ILogger<RefundQuery> logger)
    : DapperQueryBase(context, logger), 
      IRefundQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "refund_id"          AS Id,
            "payment_id"         AS PaymentId,
            "amount"             AS Amount,
            "currency"           AS Currency,
            "reason"             AS Reason,
            "status"             AS Status,
            "external_refund_id" AS ExternalRefundId,
            "failure_reason"     AS FailureReason,
            "created_at"         AS CreatedAt,
            "resolved_at"        AS ResolvedAt
        FROM "refunds"
        """;

    public async Task<RefundReadModel?> FindByIdAsync(
        Guid refundId,
        CancellationToken cancellationToken)
    {
        if (refundId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(refundId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "refund_id" = @RefundId;
                       """;

            return await connection.QueryFirstOrDefaultAsync<RefundReadModel>(
                new CommandDefinition(
                    sql,
                    new { RefundId = refundId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<RefundReadModel>> FindByPaymentIdAsync(
        Guid paymentId,
        CancellationToken cancellationToken)
    {
        if (paymentId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(paymentId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "payment_id" = @PaymentId
                       ORDER BY "created_at" DESC;
                       """;

            var result = await connection.QueryAsync<RefundReadModel>(
                new CommandDefinition(
                    sql,
                    new { PaymentId = paymentId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
}