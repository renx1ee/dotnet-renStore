using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class DeliveryTariffQuery(
    DeliveryDbContext          context,
    ILogger<DeliveryTariffQuery> logger)
    : DapperQueryBase(context, logger), IDeliveryTariffQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "tariff_id"       AS Id,
            "price_amount"    AS PriceAmount,
            "currency"        AS Currency,
            "weight_limit_kg" AS WeightLimitKg,
            "type"            AS Type,
            "description"     AS Description,
            "is_deleted"      AS IsDeleted,
            "created_at"      AS CreatedAt,
            "updated_at"      AS UpdatedAt,
            "deleted_at"      AS DeletedAt
        FROM "delivery_tariffs"
        """;

    public async Task<DeliveryTariffReadModel?> FindByIdAsync(
        int               tariffId,
        CancellationToken cancellationToken)
    {
        if (tariffId <= 0)
            throw new ArgumentOutOfRangeException(nameof(tariffId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"{BaseSql} WHERE \"tariff_id\" = @TariffId;";

            return await connection.QueryFirstOrDefaultAsync<DeliveryTariffReadModel>(
                new CommandDefinition(
                    sql,
                    new { TariffId = tariffId },
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<DeliveryTariffReadModel>> FindAllAsync(
        bool?             isDeleted,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql        = new StringBuilder(BaseSql);
            var parameters = new DynamicParameters();

            if (isDeleted.HasValue)
            {
                parameters.Add("IsDeleted", isDeleted.Value);
                sql.Append(""" WHERE "is_deleted" = @IsDeleted """);
            }

            sql.Append(""" ORDER BY "tariff_id" ASC; """);

            var result = await connection.QueryAsync<DeliveryTariffReadModel>(
                new CommandDefinition(
                    sql.ToString(),
                    parameters,
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e) { throw Wrap(e); }
    }
}