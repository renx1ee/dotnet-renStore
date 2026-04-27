using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindActiveSizePrice;
using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Ordering.Contracts.Requests;
using RenStore.Ordering.Contracts.Responses;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Messaging.Consumers;

internal sealed class GetVariantSizePriceConsumer(
    ILogger<GetVariantSizePriceConsumer> logger,
    IMediator mediator) 
    : IConsumer<GetVariantSizePriceRequest>
{
    public async Task Consume(ConsumeContext<GetVariantSizePriceRequest> context)
    {
        var msg = context.Message;

        try
        {
            logger.LogInformation(
                "Getting price for Variant {VariantId}, Size {SizeId}, CorrelationId {CorrelationId}",
                msg.VariantId, 
                msg.SizeId, 
                msg.CorrelationId);

            var variant = await mediator.Send(
                new FindVariantByIdQuery(msg.VariantId));
            
            if (variant is null)
            {
                throw new NotFoundException(
                    name: typeof(ProductVariantReadModel), msg.VariantId);
            }

            var currentPrice = await mediator.Send(
                new FindActiveVariantSizePriceBySizeIdQuery(
                    VariantId: msg.VariantId,
                    SizeId:    msg.SizeId));
            
            if (currentPrice is null)
            {
                throw new NotFoundException(
                    name: typeof(PriceHistoryReadModel), msg.SizeId);
            }
            
            await context.RespondAsync(new GetVariantPriceResponse(
                CorrelationId: msg.CorrelationId,
                PriceAmount:   currentPrice.Amount,
                Currency:      currentPrice.Currency.ToString(), // TODO:
                ProductNameSnapshot: variant.Name
            ));

            logger.LogInformation(
                "Price retrieved successfully: {Price} {Currency}",
                currentPrice.Amount,
                currentPrice.Currency);
        }
        catch (NotFoundException ex)
        {
            logger.LogWarning(ex, "Product variant size not found");
            
            await context.RespondAsync(new GetVariantPriceFailed(
                CorrelationId: msg.CorrelationId,
                Reason:        ex.Message
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting price");
            
            await context.RespondAsync(new GetVariantPriceFailed(
                CorrelationId: msg.CorrelationId,
                Reason:        $"Catalog service error: {ex.Message}"
            ));
        }
    }
}