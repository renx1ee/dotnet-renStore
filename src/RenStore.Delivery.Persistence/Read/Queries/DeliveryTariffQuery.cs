using System.ComponentModel;
using System.Text;
using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Read.Queries;

internal sealed class DeliveryTariffQuery
    : RenStore.Delivery.Persistence.Read.Base.DapperQueryBase,
      RenStore.Delivery.Application.Interfaces.IDeliveryTariffQuery
{
    private const string BaseSqlQuery =
        """
            SELECT
                ""delivery_tariff_id"" AS Id,
                ""price""              AS Price,
                ""type""               AS Type,
                ""description""        AS Description,
                ""is_deleted""         AS IsDeleted,
                ""created_date""       AS CreatedAt,
                ""updated_date""       AS UpdatedAt,
                ""deleted_date""       AS DeletedAt,
                ""delivery_order_id""  AS DeliveryOrderId
            FROM
                ""delivery_tariffs""
        """;
    
    private readonly Dictionary<DeliveryTariffSortBy, string> _sortColumnMapping = new()
    {
        { DeliveryTariffSortBy.Id, "delivery_tariff_id" },
        { DeliveryTariffSortBy.Price, "price" },
        { DeliveryTariffSortBy.Type, "type" },
        { DeliveryTariffSortBy.Description, "description" },
        { DeliveryTariffSortBy.CreatedAt, "created_date" },
        { DeliveryTariffSortBy.UpdatedAt, "updated_date" },
        { DeliveryTariffSortBy.DeletedAt, "deleted_date" }
    };

    public DeliveryTariffQuery(
        ILogger<DeliveryTariffQuery> logger,
        DeliveryDbContext context) 
        : base(context, logger)
    {
    }

    // TODO: сделать умный сорт
    
    public async Task<IReadOnlyList<DeliveryTariffReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTariffSortBy sortBy = DeliveryTariffSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null)
    {
        try
        {
            var connection = await GetOpenDbConnectionAsync(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = BuildPageRequest(
                page: page,
                pageSize: pageSize,
                descending: descending);
            
            StringBuilder sql = new StringBuilder(
                @$"
                    {BaseSqlQuery}
                ");

            if (isDeleted.HasValue)
                sql.Append($" WHERE \"is_deleted\" = @IsDeleted");

            sql.Append(@$" ORDER BY ""{columnName}"" {pageRequest.Direction}
                          LIMIT @Count
                          OFFSET @Offset;");

            var result = await connection
                .QueryAsync<DeliveryTariffReadModel>(
                    new CommandDefinition(
                        commandText: sql.ToString(),
                        parameters: new
                        {
                            Count = pageRequest.Limit,
                            Offset = pageRequest.Offset,
                            IsDeleted = isDeleted
                        },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));

            return result.AsList();
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }

    public async Task<DeliveryTariffReadModel?> FindByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new InvalidEnumArgumentException();

        try
        {
            var connection = await this.GetOpenDbConnectionAsync(cancellationToken);

            string sql =
                @$"
                    {BaseSqlQuery}
                    WHERE ""delivery_tariff_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<DeliveryTariffReadModel>(
                    new CommandDefinition(
                        commandText:sql, 
                        parameters: new { Id = id },
                        transaction: CurrentDbTransaction,
                        commandTimeout: CommandTimeoutSeconds,
                        cancellationToken: cancellationToken));
        }
        catch (PostgresException e)
        {
            throw Wrap(e);
        }
    }
    
    public async Task<DeliveryTariffReadModel> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(DeliveryTariffReadModel), id);
    }
}