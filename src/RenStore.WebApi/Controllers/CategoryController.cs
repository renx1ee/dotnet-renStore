/*using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RenStore.Application.Dto.CategoryEntity;
using RenStore.Application.Features.CategoryEntity.Commands.Create;
using RenStore.Application.Features.CategoryEntity.Commands.Delete;
using RenStore.Application.Features.CategoryEntity.Commands.Edit;
using RenStore.Application.Features.CategoryEntity.Queries.GetAllCategories;
using RenStore.Application.Features.CategoryEntity.Queries.GetCategoryById;
using RenStore.Application.Features.CategoryEntity.Queries.GetCategoryByName;

namespace RenStore.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoryController(IMapper mapper) : BaseController
{
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
    {
        var command = mapper.Map<CreateCategoryCommand>(dto);
        var result = await Mediator.Send(command);
        
        if (result != 0)
            return Created();
        
        return BadRequest();
    }

    [HttpPatch]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category/{id:int}")]
    public async Task<IActionResult> Edit(int id, [FromBody] UpdateCategoryDto dto)
    {
        var category = await Mediator.Send(new GetCategoryByIdQuery() { Id = id });
        
        if (category is null)
            return BadRequest();
        
        var command = mapper.Map<UpdateCategoryCommand>(dto);
        command.Id = id;
        
        await Mediator.Send(command); 
        return NoContent();
    }

    [HttpDelete]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(
            new DeleteCategoryCommand
            {
                Id = id
            });
        return NoContent();
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/categories")]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator
            .Send(new GetCategoriesListQuery());
        
        if (!result.Any())
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category/{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await Mediator.Send(
            new GetCategoryByIdQuery()
            {
                Id = id
            });
        
        if (result is null)
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var result = await Mediator.Send(
            new GetCategoryByNameQuery()
            {
                Name = name
            });
        
        if (result is null)
            return NotFound();
        
        return Ok(result);
    }
}*/