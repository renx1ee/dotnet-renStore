using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.WebApi.Requests;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/variants/{variantId:guid}/images")]
public sealed class VariantsMediaController : ControllerBase
{
    [HttpGet]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAll(Guid variantId)
    {
        return Ok();    
    }
    
    [HttpGet("{imageId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetById(
        Guid variantId,
        Guid imageId)
    {
        return Ok();    
    }
    
    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        Guid variantId,
        [FromBody] IReadOnlyCollection<CreateVariantImageRequest> requests)
    {
        return Created();
    }
    
    [HttpPatch("{imageId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Update(
        Guid variantId,
        Guid imageId,
        [FromBody] UpdateVariantImageRequest request)
    {
        return NoContent();
    }
    
    [HttpDelete("{imageId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(
        Guid variantId,
        Guid imageId)
    {
        return NoContent();
    }

    [HttpPatch("reorder")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Reorder(
        Guid variantId)
    {
        return NoContent();
    }
}