// Domain/Entities/City.cs
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>Справочная сущность. Управляется через EF напрямую.</summary>
public sealed class City
{
    public int     Id               { get; private set; }
    public string  Name             { get; private set; } = string.Empty;
    public string  NameRu           { get; private set; } = string.Empty;
    public string  NormalizedName   { get; private set; } = string.Empty;
    public string  NormalizedNameRu { get; private set; } = string.Empty;
    public int     CountryId        { get; private set; }
    public bool    IsDeleted        { get; private set; }

    public DateTimeOffset  CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    private City() { }

    public static City Create(
        string         name,
        string         nameRu,
        int            countryId,
        DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("NameRu cannot be empty.");

        if (countryId <= 0)
            throw new DomainException("CountryId must be greater than 0.");

        return new City
        {
            Name             = name.Trim(),
            NormalizedName   = name.Trim().ToUpperInvariant(),
            NameRu           = nameRu.Trim(),
            NormalizedNameRu = nameRu.Trim().ToUpperInvariant(),
            CountryId        = countryId,
            CreatedAt        = now,
            IsDeleted        = false
        };
    }

    public void Update(string name, string nameRu, DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot update deleted city.");

        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be empty.");

        if (string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("NameRu cannot be empty.");

        Name             = name.Trim();
        NormalizedName   = name.Trim().ToUpperInvariant();
        NameRu           = nameRu.Trim();
        NormalizedNameRu = nameRu.Trim().ToUpperInvariant();
        UpdatedAt        = now;
    }

    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot delete already deleted city.");

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
}