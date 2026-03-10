using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class VariantSizeReadModel
{
    public Guid Id { get; set; }
    public LetterSize LetterSize { get; set; }
    public decimal? Number { get; set; }
    public SizeType Type { get; set; }
    public SizeSystem System { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid VariantId { get; set; }
}