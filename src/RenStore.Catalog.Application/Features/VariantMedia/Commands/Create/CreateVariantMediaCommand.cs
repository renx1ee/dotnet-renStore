using MediatR;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Create;

public record CreateVariantMediaCommand(
    DateTimeOffset Now,
    Guid VariantId,
    string OriginalFileName,
    string StoragePath,
    long FileSizeBytes,
    bool IsMain,
    short SortOrder,
    int Weight,
    int Height) : IRequest<Guid>;
