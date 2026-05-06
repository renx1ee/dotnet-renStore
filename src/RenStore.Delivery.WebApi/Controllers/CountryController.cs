using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Features.City.Queries.FindByCountryId;
using RenStore.Delivery.Application.Features.Country.Commands.Create;
using RenStore.Delivery.Application.Features.Country.Commands.Delete;
using RenStore.Delivery.Application.Features.Country.Commands.Update;
using RenStore.Delivery.Application.Features.Country.Queries.FindAll;
using RenStore.Delivery.Application.Features.Country.Queries.FindById;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.WebApi.Requests;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/countries")]
public sealed class CountryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CountryReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindAllCountriesQuery(isDeleted),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{countryId:int}")]
    [ProducesResponseType(typeof(CountryReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] int countryId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindCountryByIdQuery(countryId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    [HttpGet("{countryId:int}/cities")]
    [ProducesResponseType(typeof(IReadOnlyList<CityReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCities(
        [FromRoute] int   countryId,
        [FromQuery] bool? isDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindCitiesByCountryIdQuery(countryId, isDeleted),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateCountryRequest request,
        CancellationToken               cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateCountryCommand(
                request.Name,
                request.NameRu,
                request.Code,
                request.PhoneCode),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { countryId = id }, id);
    }

    [HttpPut("{countryId:int}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] int                    countryId,
        [FromBody]  UpdateCountryRequest   request,
        CancellationToken                  cancellationToken)
    {
        await _mediator.Send(
            new UpdateCountryCommand(
                countryId,
                request.Name,
                request.NameRu,
                request.Code,
                request.PhoneCode),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{countryId:int}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] int countryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteCountryCommand(countryId),
            cancellationToken);

        return NoContent();
    }
}