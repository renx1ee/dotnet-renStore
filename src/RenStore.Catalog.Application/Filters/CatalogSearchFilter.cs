namespace RenStore.Catalog.Application.Filters;

public sealed record CatalogSearchFilter()
{
    public uint Page { get; init; } = 1;
    public uint PageSize { get; init; } = 25;
    public bool Descending { get; init; } = false;
    public Guid? CategoryId { get; init; }
    public Guid? SubCategoryId { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public int? ColorId { get; init; }
    public string? Search { get; init; }
    public CatalogFilterSortBy SortBy { get; init; } = CatalogFilterSortBy.Popular;
    public bool HasDiscount { get; init; }
    public int? MinDiscountPercents { get; init; }
    public bool OnlyVerifiedSellers { get; init; } = false;
    public bool? IsAvailable { get; init; }
    public int? MinReviewsCount { get; init; }
    public double? MinAverageRating { get; init; }
    /*public int? SelesCount { get; init; } */// popular
    
    
    // TODO:
    /*public string[] Sizes { get; init; } */
    /*public bool IsOriginal { get; init; }
    public bool BonusForReview { get; init; }
    public bool Cashback { get; init; }*/
    
    // TODO: по определенным категориям доп фильтры
    /*public string Season { get; init; }*/
    
    // TODO: срок доставки:
    // 1-4 часа, сегодня, завтра, послезавтра, до 3-х дней, до 5 дней
}