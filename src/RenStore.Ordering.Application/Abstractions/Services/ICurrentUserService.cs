namespace RenStore.Order.Application.Abstractions.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Role { get; }
    bool IsAuthenticated { get; }
}