namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/variants/{variantId:guid}/images")]
public sealed class VariantsMediaController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    [HttpPost]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Upload(
        Guid variantId,
        short sortOrder,
        IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        
        var result = await _mediator.Send(
            new UploadVariantImageCommand(
                VariantId: variantId,
                FileName: file.FileName,
                ContentType: file.ContentType,
                SortOrder: sortOrder,
                Stream: memoryStream));
        
        return result == Guid.Empty ? BadRequest() : Created();
    }
    
    [HttpDelete("{imageId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(
        Guid variantId,
        Guid imageId)
    {
        await _mediator.Send(new DeleteVariantImageCommand(
            VariantId: variantId,
            ImageId: imageId));
        
        return NoContent();
    }
    
    /*
       [HttpPut("reorder")]
       [Authorize(Roles = "Seller")]
       [MapToApiVersion(1)]
       public async Task<IActionResult> Reorder(
           Guid variantId)
       {
           // TODO:
           return NoContent();
       }
     
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
    
    [HttpPatch("{imageId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Update(
        Guid variantId,
        Guid imageId,
        [FromBody] UpdateVariantImageRequest request)
    {
        return NoContent();
    }*/
}