using MediatR;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Upload;

public sealed record UploadVariantImageCommand(
    Guid VariantId,
    string FileName,
    string ContentType,
    short SortOrder,
    Stream Stream)
    : IRequest<Guid>;
/*
    string StoragePath,
    long FileSizeBytes,
    bool IsMain,
    short SortOrder,
    int Weight,
    int Height*/