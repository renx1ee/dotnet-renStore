using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Abstractions;
using RenStore.Delivery.Application.Features.Address.Commands.Create;
using RenStore.Delivery.Application.Features.Address.Commands.Delete;
using RenStore.Delivery.Application.Features.Address.Commands.Edit;
using RenStore.Delivery.Application.Features.Address.Queries.FindById;
using RenStore.Delivery.Application.Features.Address.Queries.FindByUserId;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.Delivery.WebApi.Requests;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/addresses")]
/*[Authorize]*/
public sealed class AddressController(
    IMediator mediator,
    ICurrentUserService currentUser) : ControllerBase
{
    private readonly IMediator           _mediator    = mediator
                                                        ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ICurrentUserService _currentUser = currentUser
                                                        ?? throw new ArgumentNullException(nameof(currentUser));

    /// <summary>Получить адрес по ID.</summary>
    [HttpGet("{addressId:guid}")]
    [ProducesResponseType(typeof(AddressReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid addressId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindAddressByIdQuery(addressId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>Получить мои адреса.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AddressReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMy(
        [FromQuery] bool? isDeleted = false,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindAddressesByUserIdQuery(_currentUser.UserId, isDeleted),
            cancellationToken);

        return Ok(result);
    }

    /// <summary>Создать новый адрес.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAddressRequest request,
        CancellationToken               cancellationToken)
    {
        var id = await _mediator.Send(
            new CreateAddressCommand(
                UserId:          _currentUser.UserId,
                CountryId:       request.CountryId,
                CityId:          request.CityId,
                Street:          request.Street,
                HouseCode:       request.HouseCode,
                BuildingNumber:  request.BuildingNumber,
                Postcode:        request.Postcode,
                ApartmentNumber: request.ApartmentNumber,
                Entrance:        request.Entrance,
                Floor:           request.Floor),
            cancellationToken);

        return CreatedAtAction(nameof(GetById), new { addressId = id }, id);
    }

    /// <summary>Редактировать адрес.</summary>
    [HttpPut("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Edit(
        [FromRoute] Guid               addressId,
        [FromBody]  EditAddressRequest request,
        CancellationToken              cancellationToken)
    {
        await _mediator.Send(
            new EditAddressCommand(
                AddressId:       addressId,
                Street:          request.Street,
                HouseCode:       request.HouseCode,
                BuildingNumber:  request.BuildingNumber,
                Postcode:        request.Postcode,
                ApartmentNumber: request.ApartmentNumber,
                Entrance:        request.Entrance,
                Floor:           request.Floor),
            cancellationToken);

        return NoContent();
    }

    /// <summary>Удалить адрес (soft delete).</summary>
    [HttpDelete("{addressId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid addressId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteAddressCommand(addressId),
            cancellationToken);

        return NoContent();
    }
}