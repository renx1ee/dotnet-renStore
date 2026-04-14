using RenStore.Catalog.Application.Features.Category.Queries.FindCategories;
using RenStore.Catalog.Application.Features.Category.Queries.FindCategoriesWithSubcategories;
using RenStore.Catalog.Application.Features.Category.Queries.FindCategoryById;
using RenStore.Catalog.Application.Features.Category.Queries.FindSubCategories;
using RenStore.Catalog.Application.Features.Category.Queries.FindSubCategoryById;

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
                actionName: nameof(FindCategory),
                routeValues: new { categoryId },
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
                actionName: nameof(FindSubCategory),
                routeValues: new
                {
                    categoryId,
                    subCategoryId
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

    #region Queries Manage

    [HttpGet("{categoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> FindCategoryManage(
        [FromRoute] Guid categoryId,
        [FromQuery] FindCategoryManageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindCategoryByIdQuery(
                CategoryId: categoryId,
                IncludeDeleted: request.IncludeDeleted),
            cancellationToken);
        
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> FindCategoriesManage(
        [FromQuery] FindManageCategoriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindCategoriesQuery(
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy,
                IsDeleted: request.IsDeleted),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("with-sub-categories")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> FindCategoriesWithSubCategoriesManage(
        [FromQuery] FindManageCategoriesWithSubCategoriesRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindCategoriesWithSubcategoriesQuery(
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy,
                IsDeletedCategory: request.IsDeletedCategory,
                IsDeletedSubCategory: request.IsDeletedSubCategory),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("{categoryId}/sub-categories/{subCategoryId}")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> FindSubCategoryManage(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        [FromQuery] FindSubCategoryManageRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindSubCategoryByIdQuery(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId,
                IncludeDeleted: request.IncludeDeleted),
            cancellationToken);
        
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet("{categoryId}/sub-categories")]
    [ApiVersion(1)]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    public async Task<IActionResult> FindSubCategoriesManage(
        [FromRoute] Guid categoryId,
        [FromQuery] FindManageSubCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindSubCategoriesQuery(
                CategoryId: categoryId,
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy,
                IsDeleted: request.IsDeleted),
            cancellationToken);

        return Ok(result);
    }

    #endregion

    #region Queries Catalog
    
    [HttpGet("/api/v{version:apiVersion}/catalog/categories/{categoryId}")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindCategory(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindCategoryByIdQuery(
                CategoryId: categoryId),
            cancellationToken);
        
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet("/api/v{version:apiVersion}/catalog/categories")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindCategories(
        CancellationToken cancellationToken,
        [FromQuery] FindCatalogCategoriesRequest request)
    {
        var result = await _mediator.Send(
            new FindCategoriesQuery(
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("/api/v{version:apiVersion}/catalog/categories/with-sub-categories")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindCategoriesWithSubCategories(
        CancellationToken cancellationToken,
        [FromQuery] FindCatalogCategoriesWithSubCategoriesRequest request)
    {
        var result = await _mediator.Send(
            new FindCategoriesWithSubcategoriesQuery(
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet("/api/v{version:apiVersion}/catalog/categories/{categoryId}/sub-categories/{subCategoryId}")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindSubCategory(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindSubCategoryByIdQuery(
                CategoryId: categoryId,
                SubCategoryId: subCategoryId),
            cancellationToken);
        
        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet("/api/v{version:apiVersion}/catalog/categories/{categoryId}/sub-categories")]
    [ApiVersion(1)]
    [AllowAnonymous]
    public async Task<IActionResult> FindSubCategories(
        [FromRoute] Guid categoryId,
        [FromQuery] FindCatalogSubCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindSubCategoriesQuery(
                CategoryId: categoryId,
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                SortBy: request.SortBy),
            cancellationToken);

        return Ok(result);
    }
    
    #endregion
}