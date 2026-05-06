using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Features.City.Commands.Create;
using RenStore.Delivery.Application.Features.City.Commands.Delete;
using RenStore.Delivery.Application.Features.City.Commands.Update;
using RenStore.Delivery.Application.Features.City.Queries.FindById;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.WebApi.Requests;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/cities")]
public sealed class CityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{cityId:int}")]
    [ProducesResponseType(typeof(CityReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] int cityId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindCityByIdQuery(cityId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCityRequest request,
        CancellationToken            cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateCityCommand(
                request.Name,
                request.NameRu,
                request.CountryId),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { cityId = id }, id);
    }

    [HttpPut("{cityId:int}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] int               cityId,
        [FromBody]  UpdateCityRequest request,
        CancellationToken             cancellationToken)
    {
        await _mediator.Send(
            new UpdateCityCommand(cityId, request.Name, request.NameRu),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{cityId:int}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] int cityId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCityCommand(cityId), cancellationToken);
        return NoContent();
    }
}