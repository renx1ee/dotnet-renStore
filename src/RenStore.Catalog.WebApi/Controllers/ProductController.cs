using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.WebApi.Requests;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/catalog/products")]
public class ProductController : ControllerBase
{
    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request)
    {
        return Created();
    }
    
    [HttpPatch("{id:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductRequest request)
    {
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid id)
    {
        return NoContent();
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetById(Guid id)
    {
        /*return product is null ? NotFound() : Ok();*/
        return Ok();
    }
    
    [HttpGet("{article:int}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetByArticle(int article)
    {
        return Ok();
    }
}