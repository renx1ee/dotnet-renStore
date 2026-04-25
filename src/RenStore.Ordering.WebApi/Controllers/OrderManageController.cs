using Asp.Versioning;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/orders")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
public sealed class OrderManageController(IMediator mediator) : ControllerBase
{
    #region Commands

    [HttpPatch]
    [Route("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        CancellationToken cancellationToken)
    {
        return NoContent();
    }
    
    [HttpPatch]
    [Route("{orderId:guid}/approve")]
    public async Task<IActionResult> Approve(
        CancellationToken cancellationToken)
    {
        return NoContent();
    }
    
    // вернуть деньги
    [HttpPatch]
    [Route("{orderId:guid}/refund")]
    public async Task<IActionResult> Refund(
        CancellationToken cancellationToken)
    {
        return Accepted();
    }

    #endregion

    #region Queries
    
    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    [HttpGet]
    [Route("{orderId:guid}")]
    public async Task<IActionResult> GetById(
        CancellationToken cancellationToken)
    {
        return Ok();
    }

    #endregion
}