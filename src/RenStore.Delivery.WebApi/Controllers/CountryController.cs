/*using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Domain.ReadModels;

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
        [FromRoute] int countryId,
        [FromQuery] bool? isDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindCitiesByCountryIdQuery(countryId, isDeleted),
            cancellationToken);

        return Ok(result);
    }
}*/