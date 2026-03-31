using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace RenStore.Inventory.WebApi.Controllers;

[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/stock")]
public sealed class StockController : ControllerBase
{
    public async Task<IActionResult> Create()
    {
        return Ok();
    }
}