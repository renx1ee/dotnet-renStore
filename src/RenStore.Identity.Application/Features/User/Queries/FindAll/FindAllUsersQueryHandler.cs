namespace RenStore.Identity.Application.Features.User.Queries.FindAll;

internal sealed class FindAllUsersQueryHandler(
    IApplicationUserQuery userQuery,
    ILogger<FindAllUsersQueryHandler> logger)
    : IRequestHandler<FindAllUsersQuery, IReadOnlyList<ApplicationUserReadModel>>
{
    public async Task<IReadOnlyList<ApplicationUserReadModel>> Handle(
        FindAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. Page={Page} PageSize={PageSize} Status={Status}",
            typeof(FindAllUsersQuery),
            request.Page,
            request.PageSize,
            request.Status);

        var result = await userQuery.FindAllAsync(
            request.Page, request.PageSize,
            request.Descending, request.Status,
            cancellationToken);

        logger.LogInformation("Fetched {Count} users.", result.Count);

        return result;
    }
}