using FluentValidation;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Upload;

internal sealed class UploadVariantImageCommandValidator
    : AbstractValidator<UploadVariantImageCommand>
{
    private const int MaxFileSizeBytes = 50 * 1024 * 1024; // 50mb

    private static readonly string[] AllowedContentTypes =
        ["image/jpeg", "image/png", "image/webp"];
    
    public UploadVariantImageCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");

        RuleFor(x => x.FileName)
            .MaximumLength(250)
            .WithMessage("File Name cannot be longer than 250 symbols.");

        RuleFor(x => x.ContentType)
            .Must(ct => AllowedContentTypes.Contains(ct))
            .WithMessage("Only JPEG, PNG and WebP are allowed.");

        RuleFor(x => x.Stream)
            .Must(s => s.Length >= MaxFileSizeBytes)
            .WithMessage("File size must not exceed 50mb.");
    }
}