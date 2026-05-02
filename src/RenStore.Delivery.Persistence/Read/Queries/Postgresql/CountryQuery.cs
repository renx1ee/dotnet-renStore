using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class CountryQuery(
    DeliveryDbContext      context,
    ILogger<CountryQuery>  logger)
    : DapperQueryBase(context, logger), 
      ICountryQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "country_id"         AS Id,
            "name"               AS Name,
            "normalized_name"    AS NormalizedName,
            "name_ru"            AS NameRu,
            "normalized_name_ru" AS NormalizedNameRu,
            "code"               AS Code,
            "phone_code"         AS PhoneCode,
            "is_deleted"         AS IsDeleted,
            "created_at"         AS CreatedAt,
            "updated_at"         AS UpdatedAt,
            "deleted_at"         AS DeletedAt
        FROM "countries"
        """;

    public async Task<CountryReadModel?> FindByIdAsync(
        int               id,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql = $"{BaseSql} WHERE \"country_id\" = @Id;";

            return await connection.QueryFirstOrDefaultAsync<CountryReadModel>(
                new CommandDefinition(sql, new { Id = id },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<CountryReadModel>> FindAllAsync(
        bool?             isDeleted = null,
        CancellationToken cancellationToken = default)
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

            sql.Append(""" ORDER BY "name" ASC; """);

            var result = await connection.QueryAsync<CountryReadModel>(
                new CommandDefinition(sql.ToString(), parameters,
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e) { throw Wrap(e); }
    }
}