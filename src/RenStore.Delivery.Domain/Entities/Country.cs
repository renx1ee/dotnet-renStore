// Domain/Entities/Country.cs
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Справочная сущность. Управляется через EF напрямую.
/// Не является агрегатом — нет доменных событий.
/// </summary>
public sealed class Country
{
    public int     Id               { get; private set; }
    public string  Name             { get; private set; } = string.Empty;
    public string  NormalizedName   { get; private set; } = string.Empty;
    public string  NameRu           { get; private set; } = string.Empty;
    public string  NormalizedNameRu { get; private set; } = string.Empty;

    /// <summary>ISO 3166-1 alpha-2. Например: RU, DE, US.</summary>
    public string  Code             { get; private set; } = string.Empty;
    public string  PhoneCode        { get; private set; } = string.Empty;
    public bool    IsDeleted        { get; private set; }

    public DateTimeOffset  CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    private Country() { }

    public static Country Create(
        string         name,
        string         nameRu,
        string         code,
        string         phoneCode,
        DateTimeOffset now)
    {
        Validate(name, nameRu, code, phoneCode);

        return new Country
        {
            Name             = name.Trim(),
            NormalizedName   = name.Trim().ToUpperInvariant(),
            NameRu           = nameRu.Trim(),
            NormalizedNameRu = nameRu.Trim().ToUpperInvariant(),
            Code             = code.Trim().ToUpperInvariant(),
            PhoneCode        = phoneCode.Trim(),
            IsDeleted        = false,
            CreatedAt        = now
        };
    }

    public void Update(
        string         name,
        string         nameRu,
        string         code,
        string         phoneCode,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        Validate(name, nameRu, code, phoneCode);

        Name             = name.Trim();
        NormalizedName   = name.Trim().ToUpperInvariant();
        NameRu           = nameRu.Trim();
        NormalizedNameRu = nameRu.Trim().ToUpperInvariant();
        Code             = code.Trim().ToUpperInvariant();
        PhoneCode        = phoneCode.Trim();
        UpdatedAt        = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Country already deleted.");

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Cannot modify deleted country.");
    }

    private static void Validate(
        string name, string nameRu, string code, string phoneCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("NameRu cannot be empty.");

        if (string.IsNullOrWhiteSpace(code) || code.Length != 2)
            throw new DomainException("Code must be exactly 2 characters (ISO 3166-1).");

        if (string.IsNullOrWhiteSpace(phoneCode) || phoneCode.Length > 4)
            throw new DomainException("PhoneCode must be 1–4 characters.");
    }
}