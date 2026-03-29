using RenStore.Catalog.Domain.Constants;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddDetails;

internal sealed class AddVariantDetailsCommandValidator
    : AbstractValidator<AddVariantDetailsCommand>
{
    public AddVariantDetailsCommandValidator()
    {
        RuleFor(s => s.VariantId)
            .NotEmpty()
            .WithMessage("Variant ID cannot be empty guid.");
        
        RuleFor(s => s.CountryOfManufactureId)
            .GreaterThan(0)
            .WithMessage("Country ID cannot be greater than 0.");
        
        RuleFor(p => p.Description)
            .MinimumLength(CatalogConstants.ProductDetail.MinDescriptionLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxDescriptionLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant details length must be between." +
                $"{CatalogConstants.ProductDetail.MinDescriptionLength} and" +
                $"{CatalogConstants.ProductDetail.MaxDescriptionLength}");
        
        RuleFor(p => p.Composition)
            .MinimumLength(CatalogConstants.ProductDetail.MinCompositionLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxCompositionLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant composition length must be between." +
                $"{CatalogConstants.ProductDetail.MinCompositionLength} and" +
                $"{CatalogConstants.ProductDetail.MaxCompositionLength}");

        RuleFor(p => p.CaringOfThings)
            .MinimumLength(CatalogConstants.ProductDetail.MinCaringOfThingsLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxCaringOfThingsLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant Caring Of Things length must be between." +
                $"{CatalogConstants.ProductDetail.MinCaringOfThingsLength} and" +
                $"{CatalogConstants.ProductDetail.MaxCaringOfThingsLength}")
            .When(x => !string.IsNullOrEmpty(x.Description));
        
        RuleFor(p => p.ModelFeatures)
            .MinimumLength(CatalogConstants.ProductDetail.MinModelFeaturesLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxModelFeaturesLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant Model Features length must be between." +
                $"{CatalogConstants.ProductDetail.MinModelFeaturesLength} and" +
                $"{CatalogConstants.ProductDetail.MaxModelFeaturesLength}")
            .When(x => !string.IsNullOrEmpty(x.ModelFeatures));
        
        RuleFor(p => p.DecorativeElements)
            .MinimumLength(CatalogConstants.ProductDetail.MinDecorativeElementsLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxDecorativeElementsLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant Decorative Elements length must be between." +
                $"{CatalogConstants.ProductDetail.MinDecorativeElementsLength} and" +
                $"{CatalogConstants.ProductDetail.MaxDecorativeElementsLength}")
            .When(x => !string.IsNullOrEmpty(x.DecorativeElements));
        
        RuleFor(p => p.Equipment)
            .MinimumLength(CatalogConstants.ProductDetail.MinDecorativeElementsLength)
            .MaximumLength(CatalogConstants.ProductDetail.MaxDecorativeElementsLength)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "Variant Decorative Elements length must be between." +
                $"{CatalogConstants.ProductDetail.MinEquipmentLength} and" +
                $"{CatalogConstants.ProductDetail.MaxEquipmentLength}")
            .When(x => !string.IsNullOrEmpty(x.Equipment));
    }
}