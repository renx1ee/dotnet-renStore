using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Persistence.Repository.Postgresql;

public class ProductImageRepository : IProductImageRepository
{
    private readonly ApplicationDbContext _context;
    private readonly string _connectionString;
    private readonly Dictionary<ProductImageSortBy, string> _sortColumnMapping = new() 
    {
        { ProductImageSortBy.Id, "product_image_id"},
        { ProductImageSortBy.UploadedAt, "uploaded_date"}
    };

    public ProductImageRepository(
        ApplicationDbContext context,
        string connectionString)
    {
        this._context = context;
        this._connectionString = connectionString 
                                 ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public ProductImageRepository(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        this._context = context;
        this._connectionString = configuration.GetConnectionString("DefaultConnection") 
                                 ?? throw new ArgumentNullException($"DefaultConnection is null");
    }

    public async Task<Guid> CreateAsync(ProductImage image, CancellationToken cancellationToken)
    {
        var result = await _context.AddAsync(image, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return result.Entity.Id;
    }

    public async Task UpdateAsync(ProductImage image, CancellationToken cancellationToken)
    {
        var existsImage = await this.GetByIdAsync(image.Id, cancellationToken);
        
        _context.Update(image);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existsImage = await this.GetByIdAsync(id, cancellationToken);

        _context.Remove(existsImage);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProductImage>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        ProductImageSortBy sortBy = ProductImageSortBy.Id)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            pageCount = Math.Min(pageCount, 1000);
            uint offset = (page - 1) * pageCount;

            var columnMapping = _sortColumnMapping.GetValueOrDefault(sortBy, "product_image_id");
            var direction = descending ? "DESC" : "ASC";
            
            string sql =
                @$"
                    SELECT 
                        ""product_image_id""   AS Id,
                        ""original_file_name"" AS OriginalFileName,
                        ""storage_path""       AS StoragePath,
                        ""file_size_bites""    AS FileSizeBytes,
                        ""is_main""            AS IsMain,
                        ""sort_order""         AS SortOrder,
                        ""uploaded_date""      AS UploadedAt,
                        ""weight""             AS Weight,
                        ""height""             AS Height,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_images""
                    ORDER BY 
                        {columnMapping} {direction}
                    LIMIT @Count
                    OFFSET @Offset;
                ";

            return await connection.QueryAsync<ProductImage>(
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

    public async Task<ProductImage?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new NpgsqlConnection(this._connectionString);
            await connection.OpenAsync(cancellationToken);

            const string sql =
                @"
                    SELECT 
                        ""product_image_id""   AS Id,
                        ""original_file_name"" AS OriginalFileName,
                        ""storage_path""       AS StoragePath,
                        ""file_size_bites""    AS FileSizeBytes,
                        ""is_main""            AS IsMain,
                        ""sort_order""         AS SortOrder,
                        ""uploaded_date""      AS UploadedAt,
                        ""weight""             AS Weight,
                        ""height""             AS Height,
                        ""product_variant_id"" AS ProductVariantId
                    FROM
                        ""product_images""
                    WHERE 
                        ""product_image_id"" = @Id;
                ";
            
            return await connection
                .QueryFirstOrDefaultAsync<ProductImage>(
                    sql: sql, 
                    param: new { Id = id });
        }
        catch (PostgresException e)
        {
            throw new Exception($"Database error occured: {e.Message}");
        }
    }
    
    public async Task<ProductImage> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this.FindByIdAsync(id, cancellationToken) 
               ?? throw new NotFoundException(typeof(ProductImage), id);
    }

}