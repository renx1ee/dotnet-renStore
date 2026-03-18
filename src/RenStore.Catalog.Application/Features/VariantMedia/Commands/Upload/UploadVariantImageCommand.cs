namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Upload;

public sealed record UploadVariantImageCommand(
    Guid UserId,
    Guid VariantId,
    string FileName,
    string ContentType,
    short SortOrder,
    Stream Stream)
    : IRequest<Guid>;