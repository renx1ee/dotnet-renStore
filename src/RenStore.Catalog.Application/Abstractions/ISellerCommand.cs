namespace RenStore.Catalog.Application.Abstractions;

public interface ISellerVariantCommand
{
    Guid VariantId { get; }
}

public interface ISellerProductCommand
{
    Guid ProductId { get; }
}