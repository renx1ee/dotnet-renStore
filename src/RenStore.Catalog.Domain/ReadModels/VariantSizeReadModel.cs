using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class VariantSizeReadModel
{
    public Guid Id { get; init; }
    public LetterSize LetterSize { get; init; }
    public decimal? Number { get; init; }
    public SizeType Type { get; init; }
    public SizeSystem System { get; init; }
    public bool IsDeleted { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public Guid VariantId { get; init; }
}