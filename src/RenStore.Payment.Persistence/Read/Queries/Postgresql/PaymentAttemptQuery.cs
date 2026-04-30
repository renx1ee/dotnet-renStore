using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.Read.Base;

namespace RenStore.Payment.Persistence.Read.Queries.Postgresql;

internal sealed class PaymentAttemptQuery(
    PaymentDbContext context, 
    ILogger<PaymentAttemptQuery> logger)
    : DapperQueryBase(context, logger), 
      IPaymentAttemptQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "attempt_id"         AS Id,
            "payment_id"         AS PaymentId,
            "attempt_number"     AS AttemptNumber,
            "is_successful"      AS IsSuccessful,
            "failure_reason"     AS FailureReason,
            "error_code"         AS ErrorCode,
            "external_auth_code" AS ExternalAuthCode,
            "created_at"         AS CreatedAt,
            "resolved_at"        AS ResolvedAt
        FROM "payment_attempts"
        """;

    public async Task<PaymentAttemptReadModel?> FindByIdAsync(
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        if (attemptId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(attemptId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"""
                       {BaseSql}
                       WHERE "attempt_id" = @AttemptId;
                       """;

            return await connection.QueryFirstOrDefaultAsync<PaymentAttemptReadModel>(
                new CommandDefinition(
                    sql,
                    new { AttemptId = attemptId },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<IReadOnlyList<PaymentAttemptReadModel>> FindByPaymentIdAsync(
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
                       ORDER BY "attempt_number" ASC;
                       """;

            var result = await connection.QueryAsync<PaymentAttemptReadModel>(
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