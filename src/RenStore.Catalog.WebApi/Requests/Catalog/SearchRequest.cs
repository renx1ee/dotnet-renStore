namespace RenStore.Catalog.WebApi.Requests.Catalog;

public record SearchRequest(
    Guid SubCategoryId,
    decimal MaxPrice,
    decimal MinPrice,
    bool Direction
    // SortBy sortBy,
    /*Size size*/);