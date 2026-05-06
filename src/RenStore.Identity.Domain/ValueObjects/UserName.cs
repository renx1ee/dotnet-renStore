using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.ValueObjects;

public sealed class UserName : IEquatable<UserName>
{
    private const int MaxNameLenght = 100;
    private const int MinNameLenght = 2;
    
    public string FirstName { get; }
    public string LastName { get; }
    public string FullName  => $"{FirstName} {LastName}";

    private UserName(
        string firstname, 
        string lastName) 
    {
        FirstName = firstname;
        LastName = lastName;
    }

    public static UserName Create(
        string firstname, 
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstname))
            throw new DomainException("Invalid first name.");
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Invalid last name.");
        
        var normalizedFirsName = Normalize(firstname);
        var normalizedLastName = Normalize(lastName);

        Validate(normalizedFirsName, normalizedLastName);
        
        return new UserName(
            firstname: normalizedFirsName,
            lastName: normalizedLastName);
    }
    
    private static string Normalize(string value)
    {
        value = value.Trim();
        
        if(value.Length < MinNameLenght)
            throw new DomainException($"Name too short.");
        
        return char.ToUpperInvariant(value[0]) + 
               value.Substring(1).ToLowerInvariant();
    }

    private static void Validate(
        string firstname, 
        string lastName)
    {
        if(firstname.Length is > MaxNameLenght or < MinNameLenght)
            throw new DomainException($"First name length must be between {MinNameLenght} and {MaxNameLenght}.");
        
        if(lastName.Length is > MaxNameLenght or < MinNameLenght)
            throw new DomainException($"Last name length must be between {MinNameLenght} and {MaxNameLenght}.");
    }

    public bool Equals(UserName? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return FirstName == other.FirstName && 
               LastName == other.LastName;
    }

    public override bool Equals(object? obj) =>
        obj is UserName other && Equals(other);
    
    public override int GetHashCode() =>
        HashCode.Combine(FirstName, LastName);
}