using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Application.Abstractions.Queries;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.Persistence.Read.Base;

namespace RenStore.Delivery.Persistence.Read.Queries.Postgresql;

internal sealed class AddressQuery(
    DeliveryDbContext      context,
    ILogger<AddressQuery>  logger)
    : DapperQueryBase(context, logger), IAddressQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "address_id"          AS Id,
            "application_user_id" AS ApplicationUserId,
            "country_id"          AS CountryId,
            "city_id"             AS CityId,
            "street"              AS Street,
            "house_code"          AS HouseCode,
            "building_number"     AS BuildingNumber,
            "apartment_number"    AS ApartmentNumber,
            "entrance"            AS Entrance,
            "floor"               AS Floor,
            "postcode"            AS Postcode,
            "full_address_ru"     AS FullAddressRu,
            "is_deleted"          AS IsDeleted,
            "created_at"          AS CreatedAt,
            "updated_at"          AS UpdatedAt,
            "deleted_at"          AS DeletedAt
        FROM "addresses"
        """;

    public async Task<AddressReadModel?> FindByIdAsync(
        Guid              addressId,
        CancellationToken cancellationToken)
    {
        if (addressId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(addressId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            var sql = $"{BaseSql} WHERE \"address_id\" = @AddressId;";

            return await connection.QueryFirstOrDefaultAsync<AddressReadModel>(
                new CommandDefinition(
                    sql,
                    new { AddressId = addressId },
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<AddressReadModel>> FindByUserIdAsync(
        Guid              userId,
        bool?             isDeleted,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql        = new StringBuilder(
                $"{BaseSql} WHERE \"application_user_id\" = @UserId");
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);

            if (isDeleted.HasValue)
            {
                parameters.Add("IsDeleted", isDeleted.Value);
                sql.Append(""" AND "is_deleted" = @IsDeleted """);
            }

            sql.Append(""" ORDER BY "created_at" DESC; """);

            var result = await connection.QueryAsync<AddressReadModel>(
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