using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderRepository> _logger;
    private readonly string _connectionString;

    private readonly Dictionary<PaymentSortBy, string> _sortColumnMapping = new()
    {
        { PaymentSortBy.Id, "payment_id" }
    };
    
    public PaymentRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public PaymentRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(PaymentEntity payment, CancellationToken cancellationToken)
    {
        var result = await _context.Payments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return result.Entity.Id;
    }
    
    public async Task UpdateAsync(PaymentEntity payment, CancellationToken cancellationToken)
    {
        var existingPayment = await this.GetByIdAsync(payment.Id, cancellationToken);
        
        _context.Payments.Update(existingPayment);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await this.GetByIdAsync(id, cancellationToken);
        this._context.Payments.Remove(payment);
        await this._context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<PaymentEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            var offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_id");

            string sql =
                @$" 
                    SELECT 
                        ""payment_id""      AS Id,
                        ""amount""          AS InStock,
                        ""original_amount"" AS OriginalAmount,
                        ""commission""      AS Commission,
                        ""tax_amount""      AS TaxAmount,
                        ""refunded_amount"" AS RefundedAmount,
                        ""currency""        AS Currency,
                        ""is_success""      AS IsSuccess,
                        ""method""          AS Method,
                        ""method_details""  AS MethodDetails,
                        ""status""          AS Status,
                        ""error_code""      AS ErrorCode,
                        ""created_date""    AS OccuredAt,
                        ""updated_date""    AS UpdatedAt,
                        ""payment_date""    AS PaymentDate,
                        ""authorized_date"" AS AuthorizedDate,
                        ""captured_date""   AS CapturedDate,
                        ""refunded_date""   AS RefundedDate,
                        ""failed_date""     AS FailedDate,
                        ""expiry_date""     AS ExpiryDate,
                        ""order_id""        AS OrderId
                    FROM
                        ""payments""
                    ORDER BY {columnName} {direction}
                    LIMIT @Count
                    OFFSET @Offset
                ";

            return await connection
                .QueryAsync<PaymentEntity>(
                    sql, new
                    {
                        Offset = (int)offset,
                        Count = (int)pageCount
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<PaymentEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @$" 
                    SELECT 
                        ""payment_id""        AS Id,
                        ""amount""          AS InStock,
                        ""original_amount"" AS OriginalAmount,
                        ""commission""      AS Commission,
                        ""tax_amount""      AS TaxAmount,
                        ""refunded_amount"" AS RefundedAmount,
                        ""currency""        AS Currency,
                        ""is_success""      AS IsSuccess,
                        ""method""          AS Method,
                        ""method_details""  AS MethodDetails,
                        ""status""          AS Status,
                        ""error_code""      AS ErrorCode,
                        ""created_date""    AS OccuredAt,
                        ""updated_date""    AS UpdatedAt,
                        ""payment_date""    AS PaymentDate,
                        ""authorized_date"" AS AuthorizedDate,
                        ""captured_date""   AS CapturedDate,
                        ""refunded_date""   AS RefundedDate,
                        ""failed_date""     AS FailedDate,
                        ""expiry_date""     AS ExpiryDate,
                        ""order_id""        AS OrderId
                    FROM
                        ""payments""
                    WHERE
                        ""payment_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<PaymentEntity>(
                    sql, new
                    {
                        Id = id
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<PaymentEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
               ?? throw new NotFoundException(typeof(PaymentEntity), id);
    }

    public async Task<IEnumerable<PaymentEntity>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            var offset = (page - 1) * pageCount;
            var direction = descending ? "DESC" : "ASC";
            string columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "order_id");
            
            string sql =
                @$" 
                    SELECT 
                        ""payment_id""      AS Id,
                        ""amount""          AS InStock,
                        ""original_amount"" AS OriginalAmount,
                        ""commission""      AS Commission,
                        ""tax_amount""      AS TaxAmount,
                        ""refunded_amount"" AS RefundedAmount,
                        ""currency""        AS Currency,
                        ""is_success""      AS IsSuccess,
                        ""method""          AS Method,
                        ""method_details""  AS MethodDetails,
                        ""status""          AS Status,
                        ""error_code""      AS ErrorCode,
                        ""created_date""    AS OccuredAt,
                        ""updated_date""    AS UpdatedAt,
                        ""payment_date""    AS PaymentDate,
                        ""authorized_date"" AS AuthorizedDate,
                        ""captured_date""   AS CapturedDate,
                        ""refunded_date""   AS RefundedDate,
                        ""failed_date""     AS FailedDate,
                        ""expiry_date""     AS ExpiryDate,
                        ""order_id""        AS OrderId
                    FROM
                        ""payments""
                    WHERE
                        ""order_id"" = @Id
                    ORDER BY {columnName} {direction}
                    LIMIT @Limit
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<PaymentEntity>(
                    sql, new
                    {
                        Id = orderId,
                        Count = pageCount,
                        Offset = offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<IEnumerable<PaymentEntity>> GetByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken,
        PaymentSortBy sortBy = PaymentSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false)
    {
        var result = await this.FindByOrderIdAsync(
            orderId: orderId, 
            cancellationToken: cancellationToken,
            sortBy: sortBy,
            pageCount: pageCount,
            page: page,
            descending: descending);

        if(result is null || !result.Any())
               throw new NotFoundException(typeof(PaymentEntity), orderId);

        return result;
    }
}