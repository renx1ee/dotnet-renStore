namespace RenStore.Catalog.Application.Service;

public interface ICurrentUserService
{
    Guid? UserId { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}