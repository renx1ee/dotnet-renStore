using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Domain.ReadModels;
using RenStore.Identity.Persistence.Read.Queries.Base;

namespace RenStore.Identity.Persistence.Read.Queries.Postgresql;

internal sealed class ApplicationRoleQuery(
    IdentityDbContext    context,
    ILogger<ApplicationRoleQuery>   logger)
    : DapperQueryBase(context, logger), IApplicationRoleQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            "role_id"         AS Id,
            "name"            AS Name,
            "normalized_name" AS NormalizedName,
            "description"     AS Description,
            "is_deleted"      AS IsDeleted,
            "created_at"      AS CreatedAt,
            "updated_at"      AS UpdatedAt,
            "deleted_at"      AS DeletedAt
        FROM "roles"
        """;

    public async Task<RoleReadModel?> FindByIdAsync(
        Guid              roleId,
        CancellationToken cancellationToken)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(roleId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<RoleReadModel>(
                new CommandDefinition(
                    $"{BaseSql} WHERE \"role_id\" = @RoleId;",
                    new { RoleId = roleId },
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<RoleReadModel>> FindAllAsync(
        bool?             isDeleted = false,
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

            var result = await connection.QueryAsync<RoleReadModel>(
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