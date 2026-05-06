using Microsoft.Extensions.Logging;
using RenStore.Identity.Application.Abstractions.Queries;
using RenStore.Identity.Domain.Aggregates.Role;
using RenStore.Identity.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Application.Features.User.Commands.AssignRole;

internal sealed class AssignRoleCommandHandler(
    IApplicationUserRepository userRepository,
    IApplicationRoleQuery      roleQuery,
    ILogger<AssignRoleCommandHandler> logger)
    : IRequestHandler<AssignRoleCommand>
{
    public async Task Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. UserId={UserId} RoleId={RoleId}",
            nameof(AssignRoleCommand), request.UserId, request.RoleId);

        _ = await roleQuery.FindByIdAsync(request.RoleId, cancellationToken)
            ?? throw new NotFoundException(typeof(ApplicationRole), request.RoleId);

        var user = await userRepository.GetAsync(request.UserId, cancellationToken)
                   ?? throw new NotFoundException(typeof(ApplicationUser), request.UserId);

        user.AssignRole(request.RoleId, DateTimeOffset.UtcNow);
        await userRepository.SaveAsync(user, cancellationToken);

        logger.LogInformation(
            "Role assigned. UserId={UserId} RoleId={RoleId}",
            request.UserId, request.RoleId);
    }
}