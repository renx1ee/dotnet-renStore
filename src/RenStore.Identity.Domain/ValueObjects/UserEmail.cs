using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.ValueObjects;

public sealed record UserEmail
{
    private const int MaxTotalLength = 254;
    private const int MinLenght = 6;
    private const int MaxLocalLength = 64;
    private const int MaxDomainLength = 255;
    
    public string Email { get; }
    public string NormalizedEmail { get; }

    private UserEmail(
        string email)
    {
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
    }

    public static UserEmail Create(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty.");
        
        email = email.Trim();
        
        if(email.Length is < MinLenght or > MaxTotalLength)
            throw new DomainException(
                $"Email length must be between {MinLenght} and {MaxTotalLength}.");

        var parts = email.Split('@');

        if (parts.Length != 2)
            throw new DomainException("Email mast contains a symbol '@'.");

        var local = parts[0];
        var domain = parts[1];
        
        if(local.Length == 0 || local.Length > MaxLocalLength)
            throw new DomainException("Invalid local email part length.");
        
        if(domain.Length == 0 || domain.Length > MaxDomainLength)
            throw new DomainException("Invalid domain email part length.");
        
        if(!domain.Contains('.') ||
            domain.StartsWith('.') || 
            domain.EndsWith('.') || 
            domain.Contains(".."))
            throw new DomainException("Email must contains a dot.");
        
        return new UserEmail(email);
    }
}