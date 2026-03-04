using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.WebApi.Requests;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/catalog")]
public sealed class VariantsController : ControllerBase
{
    [HttpPost("products/{productId:guid}/variants")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(Guid productId)
    {
        return Created();
    }
    
    [HttpGet("products/{productId:guid}/variants")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetByProductId(Guid productId)
    {
        return Ok();
    }
    
    [HttpGet("/variants/{variantId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetById(Guid variantId)
    {
        return Ok();
    }
    
    [HttpPut("variants/{variantId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Update(
        Guid variantId,
        [FromBody] UpdateProductVariantRequest request)
    {
        return NoContent();
    }
    
    [HttpDelete("products/{productId:guid}/variants")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid variantId)
    {
        return NoContent();
    }
    
    [HttpPost("variants/{variantId:guid}/sizes")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddSize(Guid variantId)
    {
        return Created();
    }
    
    [HttpPut("variants/{variantId:guid}/sizes")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateSize(Guid variantId)
    {
        return NoContent();
    }
    
    [HttpPut("variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetPriceHistory(
        Guid variantId, 
        Guid sizeId)
    {
        return Ok();
    }
}