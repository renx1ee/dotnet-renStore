using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Repositories;

public class DeliveryTariffQuery
    (ILogger<DeliveryTariffQuery> logger,
    ApplicationDbContext context)
    : RenStore.Delivery.Application.Interfaces.IDeliveryTariffQuery
{
    private const uint MaxPageSize = 1000;
    private const int CommandTimeoutSeconds = 30;
    
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

    private readonly ILogger<DeliveryTariffQuery> _logger = logger 
                                                            ?? throw new ArgumentNullException(nameof(logger));
    private readonly ApplicationDbContext _context        = context 
                                                            ?? throw new ArgumentNullException(nameof(context));

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

    private DbTransaction? CurrentDbTransaction =>
        this._context.Database.CurrentTransaction?.GetDbTransaction();

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
            var connection = await GetOpenDbConnection(cancellationToken);

            if (!_sortColumnMapping.TryGetValue(sortBy, out var columnName))
                throw new ArgumentOutOfRangeException(nameof(sortBy));

            var pageRequest = this.BuildPageRequest(
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
            var connection = await this.GetOpenDbConnection(cancellationToken);

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

    private DeliveryTariffPageRequest BuildPageRequest(uint page, uint pageSize, bool descending)
    {
        if (page == 0)
            throw new ArgumentOutOfRangeException(nameof(page));
        
        pageSize = Math.Min(pageSize, MaxPageSize);
        var offset = (page - 1) * pageSize;
        var direction = descending ? "DESC" : "ASC";
        
        return new DeliveryTariffPageRequest(
            Limit: (int)pageSize,
            Offset: (int)offset,
            Direction: direction);
    }

    private async Task<DbConnection> GetOpenDbConnection(CancellationToken cancellationToken)
    {
        var connecton = this._context.Database.GetDbConnection();

        if (connecton.State != ConnectionState.Open)
            await connecton.OpenAsync(cancellationToken);

        return connecton;
    }

    private DataException Wrap(Exception e)
    {
        _logger.LogError(e, "Database error occured.");
        return new DataException("Database error occured.", e);
    }
}

readonly record struct DeliveryTariffPageRequest(int Limit, int Offset, string Direction);