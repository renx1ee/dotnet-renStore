using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;

namespace RenStore.Persistence.Repository.Postgresql;

public class SellerImageRepository : ISellerImageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<SellerImageSortBy, string> _sortColumnMapping = new()
    {
        { SellerImageSortBy.Id, "seller_image_id" },
        { SellerImageSortBy.UploadedAt, "seller_image_upload_date" },
    };

    public SellerImageRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public SellerImageRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }
    
    public async Task<Guid> CreateAsync(
        SellerImageEntity image, 
        CancellationToken cancellationToken)
    {
        var result = await _context.SellerImages.AddAsync(image, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(
        SellerImageEntity image,
        CancellationToken cancellationToken)
    {
        var existingAttribute = await this.GetByIdAsync(image.Id, cancellationToken);

        _context.SellerImages.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var image = await this.GetByIdAsync(id, cancellationToken);
        
        _context.SellerImages.Remove(image);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<SellerImageEntity>> FindAllAsync(
        CancellationToken cancellationToken, 
        uint pageCount = 25, 
        uint page = 1, 
        bool descending = false,
        SellerImageSortBy sortBy = SellerImageSortBy.Id)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;
            string direction = descending ? "DESC" : "ASC";
            var columnName = _sortColumnMapping.GetValueOrDefault(sortBy, "price_history_id");

            string sql =
                $@"
                    SELECT 
                        ""seller_image_id""    AS Id,
                        ""original_file_name"" AS OriginalFileName,
                        ""storage_path""       AS StoragePath,
                        ""file_size_bites""    AS FileSizeBytes,
                        ""is_main""            AS IsMain,
                        ""sort_order""         AS SortOrder,
                        ""uploaded_date""      AS UploadedAt,
                        ""weight""             AS Weight,
                        ""height""             AS Height,
                        ""seller_id""          AS SellerId
                    FROM
                        ""seller_images""
                    ORDER BY {columnName} {direction} 
                    LIMIT @Count 
                    OFFSET @Offset;
                ";

            return await connection
                .QueryAsync<SellerImageEntity>(
                    sql, new
                    {
                        Count = (int)pageCount,
                        Offset = (int)offset
                    });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }

    public async Task<SellerImageEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql = 
                @"
                    SELECT 
                        ""seller_image_id""    AS Id,
                        ""original_file_name"" AS OriginalFileName,
                        ""storage_path""       AS StoragePath,
                        ""file_size_bites""    AS FileSizeBytes,
                        ""is_main""            AS IsMain,
                        ""sort_order""         AS SortOrder,
                        ""uploaded_date""      AS UploadedAt,
                        ""weight""             AS Weight,
                        ""height""             AS Height,
                        ""seller_id""          AS SellerId
                    FROM
                        ""seller_images""
                    WHERE 
                        ""seller_image_id"" = @Id;
                ";

            return await connection
                .QueryFirstOrDefaultAsync<SellerImageEntity>(
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

    public async Task<SellerImageEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException(typeof(SellerImageEntity), id);
    }
}