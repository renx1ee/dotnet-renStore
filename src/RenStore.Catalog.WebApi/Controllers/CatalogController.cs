/*using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.WebApi.Requests;

namespace RenStore.Catalog.WebApi.Controllers;

// TODO: "pagination": {
//   "page": 1,
//   "size": 20,
//   "total": 340,
//   "totalPages": 17
// }

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/[controller]")]
public class CatalogController : ControllerBase
{
    // TODO: /api/catalog/products?page=1&size=20&categoryId=...&minPrice=...&maxPrice=...&sortBy=price&sortDir=asc
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/products")]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    // TODO: /api/catalog/products/search?q=nike&page=1&size=20
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/catalog/products/search?")]
    public async Task<IActionResult> Search(
        
        [FromBody] SearchRequest request)
    {
        return Ok();
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/catalog/categories/tree")]
    public async Task<IActionResult> GetCategories()
    {
        return Ok();
    }
    
    // TODO: /api/catalog/categories/{id}/products?page=1&size=20
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/catalog/categories/{categoryId:guid}/products")]
    public async Task<IActionResult> GetByCategory()
    {
        return Ok();
    }
}*/