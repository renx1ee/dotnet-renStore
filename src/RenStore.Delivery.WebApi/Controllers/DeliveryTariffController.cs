// Controllers/DeliveryTariffController.cs

using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Change;
using RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Create;
using RenStore.Delivery.Application.Features.DeliveryTariff.Commands.Delete;
using RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindAll;
using RenStore.Delivery.Application.Features.DeliveryTariff.Queries.FindById;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.WebApi.Requests;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/delivery-tariffs")]
public sealed class DeliveryTariffController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DeliveryTariffReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindAllDeliveryTariffsQuery(isDeleted),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{tariffId:int}")]
    [ProducesResponseType(typeof(DeliveryTariffReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] int tariffId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindDeliveryTariffByIdQuery(tariffId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDeliveryTariffRequest request,
        CancellationToken                      cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateDeliveryTariffCommand(
                request.PriceAmount,
                request.Currency,
                request.WeightLimitKg,
                request.Type,
                request.Description),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { tariffId = id }, id);
    }

    [HttpPatch("{tariffId:int}/price")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePrice(
        [FromRoute] int                       tariffId,
        [FromBody]  ChangeTariffPriceRequest  request,
        CancellationToken                     cancellationToken)
    {
        await _mediator.Send(
            new ChangeTariffPriceCommand(tariffId, request.PriceAmount, request.Currency),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{tariffId:int}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] int tariffId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteDeliveryTariffCommand(tariffId),
            cancellationToken);

        return NoContent();
    }
}