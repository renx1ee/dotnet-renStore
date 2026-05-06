using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Aggregates.Address;

/// <summary>
/// Агрегат адреса пользователя.
/// Самостоятельный жизненный цикл, не зависит от DeliveryOrder.
/// </summary>
public sealed class Address
{
    public Guid    Id                { get; private set; }
    public Guid    ApplicationUserId { get; private set; }
    public int     CountryId         { get; private set; }
    public int     CityId            { get; private set; }
    public string  Street            { get; private set; } = string.Empty;
    public string  HouseCode         { get; private set; } = string.Empty;
    public string  BuildingNumber    { get; private set; } = string.Empty;
    public string  ApartmentNumber   { get; private set; } = string.Empty;
    public string  Entrance          { get; private set; } = string.Empty;
    public int?    Floor             { get; private set; }

    /// <summary>
    /// Индекс — обязателен для Почты России.
    /// </summary>
    public string  Postcode          { get; private set; } = string.Empty;

    public string  FullAddressRu     { get; private set; } = string.Empty;

    public bool            IsDeleted  { get; private set; }
    public DateTimeOffset  CreatedAt  { get; private set; }
    public DateTimeOffset? UpdatedAt  { get; private set; }
    public DateTimeOffset? DeletedAt  { get; private set; }

    private Address() { }

    public static Address Create(
        Guid           userId,
        int            countryId,
        int            cityId,
        string         street,
        string         houseCode,
        string         buildingNumber,
        string         postcode,
        DateTimeOffset now,
        string?        apartmentNumber = null,
        string?        entrance        = null,
        int?           floor           = null)
    {
        if (userId == Guid.Empty)
            throw new DomainException("UserId cannot be empty.");

        if (countryId <= 0)
            throw new DomainException("CountryId must be greater than 0.");

        if (cityId <= 0)
            throw new DomainException("CityId must be greater than 0.");

        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be empty.");

        if (string.IsNullOrWhiteSpace(houseCode))
            throw new DomainException("HouseCode cannot be empty.");

        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new DomainException("BuildingNumber cannot be empty.");

        if (string.IsNullOrWhiteSpace(postcode) || postcode.Length != 6)
            throw new DomainException("Postcode must be exactly 6 digits.");

        var address = new Address
        {
            Id                = Guid.NewGuid(),
            ApplicationUserId = userId,
            CountryId         = countryId,
            CityId            = cityId,
            Street            = street.Trim(),
            HouseCode         = houseCode.Trim(),
            BuildingNumber    = buildingNumber.Trim(),
            Postcode          = postcode.Trim(),
            ApartmentNumber   = apartmentNumber?.Trim() ?? string.Empty,
            Entrance          = entrance?.Trim()        ?? string.Empty,
            Floor             = floor,
            CreatedAt         = now,
            IsDeleted         = false
        };

        address.BuildFullAddress();
        return address;
    }

    public void Edit(
        string         street,
        string         houseCode,
        string         buildingNumber,
        string         postcode,
        DateTimeOffset now,
        string?        apartmentNumber = null,
        string?        entrance        = null,
        int?           floor           = null)
    {
        EnsureNotDeleted();

        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be empty.");

        if (string.IsNullOrWhiteSpace(houseCode))
            throw new DomainException("HouseCode cannot be empty.");

        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new DomainException("BuildingNumber cannot be empty.");

        if (string.IsNullOrWhiteSpace(postcode) || postcode.Length != 6)
            throw new DomainException("Postcode must be exactly 6 digits.");

        Street          = street.Trim();
        HouseCode       = houseCode.Trim();
        BuildingNumber  = buildingNumber.Trim();
        Postcode        = postcode.Trim();
        ApartmentNumber = apartmentNumber?.Trim() ?? string.Empty;
        Entrance        = entrance?.Trim()        ?? string.Empty;
        Floor           = floor;
        UpdatedAt       = now;

        BuildFullAddress();
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }

    private void BuildFullAddress()
    {
        var parts = new List<string>
        {
            $"{Postcode},",
            Street,
            $"д. {BuildingNumber}",
        };

        if (!string.IsNullOrWhiteSpace(HouseCode))
            parts.Add($"к. {HouseCode}");

        if (!string.IsNullOrWhiteSpace(ApartmentNumber))
            parts.Add($"кв. {ApartmentNumber}");

        if (!string.IsNullOrWhiteSpace(Entrance))
            parts.Add($"подъезд {Entrance}");

        if (Floor.HasValue)
            parts.Add($"эт. {Floor}");

        FullAddressRu = string.Join(", ", parts);
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Cannot modify deleted address.");
    }
}