namespace RenStore.Identity.Application.Features.User.Queries.FindUserById;

internal sealed class FindUserByIdQueryHandler(
    IApplicationUserQuery userQuery,
    ILogger<FindUserByIdQueryHandler> logger)
    : IRequestHandler<FindUserByIdQuery, ApplicationUserReadModel?>
{
    public async Task<ApplicationUserReadModel?> Handle(
        FindUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Query}. UserId={UserId}",
            nameof(FindUserByIdQuery), request.UserId);

        var result = await userQuery.FindByIdAsync(request.UserId, cancellationToken);

        if (result is null)
            logger.LogWarning("User not found. UserId={UserId}", request.UserId);

        return result;
    }
}