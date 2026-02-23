using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Identity.Domain.ValueObjects;

public sealed record UserPhone
{
    private const int MaxPhoneLength = 15;
    private const int MinPhoneLength = 5;
    
    public string Value { get; }
    
    private UserPhone(string value)
    {
        Value = value;
    }

    public static UserPhone Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("Phone number cannot be null or empty.");

        var normalizedPhone = Normalize(phoneNumber);

        ValidateNumber(normalizedPhone);
        
        return new UserPhone(normalizedPhone);
    }

    private static string Normalize(string phone)
    {
        phone = phone.Trim()
            .Replace(" ", "")
            .Replace("-", "")
            .Replace("(", "")
            .Replace(")", "");
        
        return phone;
    }

    private static void ValidateNumber(string phone)
    {
        if (!phone.StartsWith('+'))
            throw new DomainException("Phone number must start with '+'.");

        var digits = phone[1..];
        
        if(!digits.All(char.IsDigit))
            throw new DomainException("Phone number must contains only digits after '+'.");
        
        if(digits.Length is < MinPhoneLength or > MaxPhoneLength)
            throw new DomainException(
                $"Phone number length must contain between {MinPhoneLength} and {MaxPhoneLength} digits.");
    }
}