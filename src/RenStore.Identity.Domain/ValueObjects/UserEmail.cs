using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.ValueObjects;

public sealed record UserEmail
{
    private const int MaxTotalLength  = 254;
    private const int MinLenght       = 6;
    private const int MaxLocalLength  = 64;
    private const int MaxDomainLength = 255;
    
    public string Value { get; }

    private UserEmail(string value)
    {
        Value = value;
    }

    public static UserEmail Create(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");
        
        email = email.Trim();
        
        if(email.Length is < MinLenght or > MaxTotalLength)
        {
            throw new DomainException(
                $"Email length must be between {MinLenght} and {MaxTotalLength}.");
        }
        
        var parts = email.Split('@');

        if (parts.Length != 2)
            throw new DomainException("Email mast contains a symbol '@'.");

        var local = parts[0];
        var domain = parts[1];
        
        if (string.IsNullOrWhiteSpace(local))
            throw new DomainException("Invalid local part.");

        if (string.IsNullOrWhiteSpace(domain))
            throw new DomainException("Invalid domain.");

        if (!domain.Contains('.'))
            throw new DomainException("Invalid domain.");
        
        var normalized =
            $"{local.ToLowerInvariant()}@{domain.ToLowerInvariant()}";
        
        return new UserEmail(email);
    }
    
    public override string ToString() => Value;
}