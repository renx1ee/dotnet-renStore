using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.Aggregates.User.Rules;

internal static class UserRules
{
    internal static void ValidateCreatingUser(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        ValidateUserName(firstName: firstName, lastName: lastName);
        ValidateEmail(email);
        ValidatePasswordHash(passwordHash);
    }
    
    internal static void ValidateUserName(
        string firstName,
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be null or white space.");
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be null or white space.");
    }

    internal static void ValidateEmail(
        string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be null or white space.");
    }

    internal static void ValidatePasswordHash(
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Phone number cannot be null or white space.");
    }
}