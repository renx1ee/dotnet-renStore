using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ReadModels;
using RenStore.Identity.Persistence.Read.Queries.Base;

namespace RenStore.Identity.Persistence.Read.Queries.Postgresql;

internal sealed class ApplicationUserQuery(
    IdentityDbContext              context,
    ILogger<ApplicationUserQuery>  logger)
    : DapperQueryBase(context, logger), IApplicationUserQuery
{
    private static readonly string BaseSql =
        """
        SELECT
            u."user_id"            AS Id,
            u."first_name"         AS FirstName,
            u."last_name"          AS LastName,
            u."full_name"          AS FullName,
            u."email"              AS Email,
            u."email_confirmed"    AS EmailConfirmed,
            u."password_hash"      AS PasswordHash,
            u."phone"              AS Phone,
            u."phone_confirmed"    AS PhoneConfirmed,
            u."access_failed_count" AS AccessFailedCount,
            u."lockout_end"        AS LockoutEnd,
            u."status"             AS Status,
            u."created_at"         AS CreatedAt,
            u."updated_at"         AS UpdatedAt,
            u."deleted_at"         AS DeletedAt
        FROM "users" u
        """;

    public async Task<ApplicationUserReadModel?> FindByIdAsync(
        Guid              userId,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<ApplicationUserReadModel>(
                new CommandDefinition(
                    $"{BaseSql} WHERE u.\"user_id\" = @UserId;",
                    new { UserId = userId },
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<ApplicationUserReadModel?> FindByEmailAsync(
        string            email,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            return await connection.QueryFirstOrDefaultAsync<ApplicationUserReadModel>(
                new CommandDefinition(
                    $"{BaseSql} WHERE u.\"email\" = @Email;",
                    new { Email = email.Trim().ToLowerInvariant() },
                    commandTimeout:    CommandTimeoutSeconds,
                    transaction:       CurrentDbTransaction,
                    cancellationToken: cancellationToken));
        }
        catch (PostgresException e) { throw Wrap(e); }
    }

    public async Task<IReadOnlyList<ApplicationUserReadModel>> FindAllAsync(
        uint                   page = 1,
        uint                   pageSize = 25,
        bool                   descending = true,
        ApplicationUserStatus? status = null,
        CancellationToken      cancellationToken = default)
    {
        var pageRequest = BuildPageRequest(page, pageSize, descending);

        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);
            var sql        = new StringBuilder(BaseSql);
            var parameters = new DynamicParameters();
            parameters.Add("Count",  pageRequest.Limit);
            parameters.Add("Offset", pageRequest.Offset);

            if (status.HasValue)
            {
                parameters.Add("Status", status.Value.ToString());
                sql.Append(""" WHERE u."status" = @Status """);
            }

            sql.Append($""" ORDER BY u."created_at" {pageRequest.Direction} LIMIT @Count OFFSET @Offset; """);

            var result = await connection.QueryAsync<ApplicationUserReadModel>(
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