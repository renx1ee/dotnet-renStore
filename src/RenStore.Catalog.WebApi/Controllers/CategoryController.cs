namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/categories")]
public sealed class CategoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands
    
    [HttpPost]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var categoryId = await _mediator.Send(
            new CreateCategoryCommand(
                Name: request.Name,
                NameRu: request.NameRu,
                IsActive: request.IsActive,
                Description: request.Description), 
            cancellationToken);

        return categoryId == Guid.Empty
            ? BadRequest()
            : CreatedAtAction(
                actionName: nameof(FindCategoryById),
                routeValues: new { categoryId = categoryId},
                value: new { Id = categoryId });
    }
    
    [HttpPatch("{categoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> Update(
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateCategoryCommand(
                CategoryId: categoryId,
                Name: request.Name,
                NameRu: request.NameRu,
                Description: request.Description), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/activate")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> Activate(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ActivateCategoryCommand(
                CategoryId: categoryId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/deactivate")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> Deactivate(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeactivateCategoryCommand(
                CategoryId: categoryId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpDelete("{categoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> Delete(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SoftDeleteCategoryCommand(
                CategoryId: categoryId),
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/restore")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> Restore(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RestoreCategoryCommand(
                CategoryId: categoryId),
            cancellationToken);
        
        return NoContent();
    }

    
    [HttpPost("{categoryId}/sub-categories")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> CreateSubCategory(
        [FromRoute] Guid categoryId,
        [FromBody] CreateSubCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var subCategoryId = await _mediator.Send(
            new CreateSubCategoryCommand(
                CategoryId: categoryId,
                Name: request.Name,
                NameRu: request.NameRu,
                IsActive: request.IsActive,
                Description: request.Description), 
            cancellationToken);

        return subCategoryId == Guid.Empty
            ? BadRequest()
            : CreatedAtAction(
                actionName: nameof(FindCategoryById),
                routeValues: new
                {
                    Id = subCategoryId, 
                    CategoryId = categoryId, 
                    Version = "1"
                },
                value: new
                {
                    Id = subCategoryId, 
                    CategoryId = categoryId
                });
    }
    
    [HttpPatch("{categoryId}/sub-categories/{subCategoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> UpdateSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        [FromBody] UpdateSubCategoryRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateSubCategoryCommand(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId,
                Name: request.Name,
                NameRu: request.NameRu,
                Description: request.Description), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/sub-categories/{subCategoryId}/activate")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> ActivateSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ActivateSubCategoryCommand(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/sub-categories/{subCategoryId}/deactivate")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> DeactivateSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeactivateSubCategoryCommand(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId), 
            cancellationToken);
        
        return NoContent();
    } 
    
    [HttpDelete("{categoryId}/sub-categories/{subCategoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> DeleteSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SoftDeleteSubCategoryCommand(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{categoryId}/sub-categories/{subCategoryId}/restore")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> RestoreSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RestoreSubCategoryCommand(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId), 
            cancellationToken);
        
        return NoContent();
    }
    
    #endregion

    #region Queries

    [HttpGet("{categoryId}")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindCategoryById(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpGet("{categoryId}/sub-categories/{subCategoryId}")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindSubCategoryById(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    #endregion
}