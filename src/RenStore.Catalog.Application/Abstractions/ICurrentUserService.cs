namespace RenStore.Catalog.Application.Abstractions;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}