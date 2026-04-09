namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Denormalization.ChangeReviewsCount;

public sealed record ChangeReviewsCountProjectionCommand(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    int Count,
    double AverageRating)
    : IRequest;