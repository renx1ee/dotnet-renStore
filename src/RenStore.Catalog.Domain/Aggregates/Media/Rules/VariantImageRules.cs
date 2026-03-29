using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Media.Rules;

internal static class VariantImageRules
{
    internal static void UpdatedByParametersValidation(
        Guid updatedById,
        string updatedByRole)
    {
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
    }
}