/*using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RenStore.Application.Dto.Order;
using RenStore.Application.Features.DeliveryOrders.Commands.Create;
using RenStore.Application.Features.DeliveryOrders.Commands.Delete;
using RenStore.Application.Features.DeliveryOrders.Commands.Edit;
using RenStore.Application.Features.DeliveryOrders.Queries.GetAll;
using RenStore.Application.Features.DeliveryOrders.Queries.GetById;
using RenStore.Application.Features.DeliveryOrders.Queries.GetByProductId;
using RenStore.Application.Features.DeliveryOrders.Queries.GetByUserId;
using RenStore.Identity.DuendeServer.WebAPI.Data.IdentityConfigurations;

namespace RenStore.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrderController(IMapper mapper, JwtProvider jwtProvider) : BaseController
{
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/order")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto model)
    {
        string? accessToken = HttpContext.Request.Cookies["tasty-cookies"];
        if(accessToken == null)
            return Unauthorized();
        
        var userId = jwtProvider.GetUserId(accessToken);
        
        var command = mapper.Map<CreateOrderCommand>(model);
        command.ApplicationUserId = userId;
        
        var result = await Mediator.Send(command);
        if(result == Guid.Empty)
            return BadRequest();
        
        return Ok(result);
    }
    
    [HttpPatch]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/order/{id:int}")]
    public async Task<IActionResult> Edit([FromBody] UpdateOrderDto updateOrderDto)
    {
        var command = mapper.Map<UpdateOrderCommand>(updateOrderDto);
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/order/{id:int}")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        await Mediator.Send(new DeleteOrderCommand()
        {
            OrderId = id
        });
        return NoContent();
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/orders")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllOrdersQuery());
        
        if(!result.Any())
            return NotFound();
            
        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/order/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetOrderByIdQuery() { Id = id });
        
        if(result is null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/user-orders/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var result = await Mediator.Send(
            new GetOrdersByUserIdQuery()
            {
                UserId = userId
            });
        
        if(!result.Any())
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-orders/{productId:guid}")]
    public async Task<IActionResult> GetAllByProductId(Guid productId)
    {
        var result = await Mediator.Send(
            new GetOrdersByProductIdQuery()
            {
                ProductId = productId
            });
        
        if(!result.Any())
            return NotFound();
        
        return Ok(result);
    }
}*/