namespace RenStore.Catalog.Application.Filters;

public sealed record CatalogFilter()
{
    public Guid? CategoryId { get; init; }
    public Guid? SubCategoryId { get; init; }
    public uint Page { get; init; } = 1;
    public uint PageSize { get; init; } = 25;
    public bool Descending { get; init; } = false;
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int? ColorId { get; init; }
    public string? Search { get; init; }
    public CatalogFilterSortBy SortBy { get; init; } = CatalogFilterSortBy.Newest;
}