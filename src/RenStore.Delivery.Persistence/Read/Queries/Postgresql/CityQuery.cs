using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class CityQuery(
    DeliveryDbContext    context,
    ILogger<CityQuery>  logger)
    : DapperQueryBase(context, logger), 
      ICityQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "city_id"            AS Id,
            "name"               AS Name,
            "name_ru"            AS NameRu,
            "normalized_name"    AS NormalizedName,
            "normalized_name_ru" AS NormalizedNameRu,
            "is_deleted"         AS IsDeleted,
            "country_id"         AS CountryId,
            "created_at"         AS CreatedAt,
            "updated_at"         AS UpdatedAt,
            "deleted_at"         AS DeletedAt
        FROM "cities"
        """;

    public async Task<CityReadModel?> FindByIdAsync(
        int               id,
        CancellationToken cancellationToken)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql = $"{BaseSql} WHERE \"city_id\" = @Id;";

            return await connection.QueryFirstOrDefaultAsync<CityReadModel>(
                new CommandDefinition(sql, new { Id = id },
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<CityReadModel>> FindByCountryIdAsync(
        int               countryId,
        bool?             isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql        = new StringBuilder($"{BaseSql} WHERE \"country_id\" = @CountryId");
            var parameters = new DynamicParameters();
            parameters.Add("CountryId", countryId);

            if (isDeleted.HasValue)
            {
                parameters.Add("IsDeleted", isDeleted.Value);
                sql.Append(""" AND "is_deleted" = @IsDeleted """);
            }

            sql.Append(""" ORDER BY "name" ASC; """);

            var result = await connection.QueryAsync<CityReadModel>(
                new CommandDefinition(sql.ToString(), parameters,
                    commandTimeout: CommandTimeoutSeconds,
                    transaction: CurrentDbTransaction,
                    cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e) { throw Wrap(e); }
    }
}