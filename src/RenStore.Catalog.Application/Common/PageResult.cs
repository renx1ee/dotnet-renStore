namespace RenStore.Catalog.Application.Common;

public record PageResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize);