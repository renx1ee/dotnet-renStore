namespace RenStore.Identity.Domain.ReadModels;

public sealed class PendingEmailVerificationReadModel
{
    public Guid           Id        { get; set; }
    public Guid           UserId    { get; set; }
    public string         Email     { get; set; } = string.Empty;
    public Guid           Token     { get; set; }
    public bool           IsUsed    { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}