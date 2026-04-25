using Asp.Versioning;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Authorize(Roles = Roles.Buyer)] 
[Microsoft.AspNetCore.Components.Route("api/v{version:apiVersion}/orders")]
public sealed class OrderController(IMediator mediator) : ControllerBase
{
    #region Commands

    [HttpPost]
    public async Task<IActionResult> Create(
        CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpPatch]
    [Route("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    #endregion

    #region Queries
    
    // список своих заказов
    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    // статус своего заказа
    [HttpGet]
    [Route("{orderId:guid}")] 
    public async Task<IActionResult> GetById(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    #endregion
}