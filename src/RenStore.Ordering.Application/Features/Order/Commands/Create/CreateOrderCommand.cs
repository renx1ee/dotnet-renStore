using FluentValidation;
using MediatR;
using RenStore.Order.Domain.Constants;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Application.Features.Order.Commands.Create;

public sealed record CreateOrderCommand(
    Guid     CustomerId,
    Guid     VariantId,
    Guid     SizeId,
    int      Quantity) // TODO: проверка доступности
    : IRequest<Guid>;

/*string   ShippingAddress,
decimal  PriceAmount,
Currency Currency,
string   ProductNameSnapshot*/

internal sealed class CreateOrderCommandValidator
    : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID cannot be empty guid.");
        
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage("VariantId ID cannot be empty guid.");
        
        RuleFor(x => x.SizeId)
            .NotEmpty()
            .WithMessage("SizeId ID cannot be empty guid.");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .LessThan(OrderConstants.OrderItem.MaxQuantity)
            .WithMessage($"Quantity must be between 0 and " +
                         $"{OrderConstants.OrderItem.MaxQuantity}.");

        /*
        RuleFor(x => x.ShippingAddress)
            .NotEmpty()
            .MaximumLength(OrderConstants.Order.ShippingAddressMaxLength)
            .MinimumLength(OrderConstants.Order.ShippingAddressMaxLength)
            .WithMessage("Shipping address must be between " +
                       $"{OrderConstants.Order.ShippingAddressMaxLength} and " +
                       $"{OrderConstants.Order.ShippingAddressMinLength} characters.");
                                   
         RuleFor(x => x.PriceAmount)
            .GreaterThan(0)
            .WithMessage("PriceAmount must be more then 0 and ");
        
        RuleFor(x => x.ProductNameSnapshot)
            .NotEmpty()
            .WithMessage($"{nameof(CreateOrderCommand.ProductNameSnapshot)} cannot be empty.")
            .MaximumLength(OrderConstants.OrderItem.ProductNameSnapshotMaxLength)
            .WithMessage("PriceAmount must be between 0 and " +
                         $"{OrderConstants.OrderItem.ProductNameSnapshotMaxLength}.");*/
    }
}