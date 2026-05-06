namespace RenStore.Identity.Domain.Interfaces;

public interface IApplicationUserRepository
{
    Task<ApplicationUser?> GetAsync(
        Guid              userId,
        CancellationToken cancellationToken);

    Task SaveAsync(
        ApplicationUser user,
        CancellationToken               cancellationToken);
}