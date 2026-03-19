using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.WebApi.Requests.Variant;

public sealed record UpdateDetailsRequest(
    string? Description = null,
    string? Composition = null,
    string? ModelFeatures = null,
    string? DecorativeElements = null,
    string? Equipment = null,
    string? CaringOfThings = null,
    TypeOfPacking? TypeOfPacking = null);