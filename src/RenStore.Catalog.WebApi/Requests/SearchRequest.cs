namespace RenStore.Catalog.WebApi.Requests;

public record SearchRequest(
    Guid SubCategoryId,
    decimal MaxPrice,
    decimal MinPrice,
    bool Direction
    // SortBy sortBy,
    /*Size size*/);