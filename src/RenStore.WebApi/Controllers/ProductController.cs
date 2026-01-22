/*using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Application.Dto.Product;
using RenStore.Application.Features.Product.Commands.Create.Clothes;
using RenStore.Application.Features.Product.Commands.Create.Shoes;
using RenStore.Application.Features.Product.Commands.Delete;
using RenStore.Application.Features.Product.Commands.Edit;
using RenStore.Application.Features.Product.Queries.GetByArticle;
using RenStore.Application.Features.Product.Queries.GetById;
using RenStore.Application.Features.Product.Queries.GetByName;
using RenStore.Application.Features.Product.Queries.GetByNovelty;
using RenStore.Application.Features.Product.Queries.GetBySellerId;
using RenStore.Application.Features.Product.Queries.GetMinimizedProducts;
using RenStore.Application.Features.Product.Queries.GetSortedByPrice;
using RenStore.Application.Features.Product.Queries.GetSortedByRating;

namespace RenStore.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class ProductController(IMapper mapper) : BaseController
{
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-clothes")]
    public async Task<IActionResult> CreateClothes([FromBody] CreateClothesProductDto model)
    {
        var command = mapper.Map<CreateClothesProductCommand>(model);
        var result = await Mediator.Send(command);

        if (result == Guid.Empty)
            return BadRequest();

        return Created();
    }
    
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-shoes")]
    public async Task<IActionResult> CreateShoes([FromBody] CreateShoesProductDto model)
    {
        var command = mapper.Map<CreateShoesProductCommand>(model);
        var result = await Mediator.Send(command);

        if (result == Guid.Empty)
            return BadRequest();
        
        return Ok();
    }
    
    [HttpPut]
    [MapToApiVersion(1)]
    // TODO: доделать
    [Route("/api/v{version:apiVersion}/product")]
    public async Task<IActionResult> Edit([FromBody] UpdateProductDto model)
    {
        var command = mapper.Map<UpdateProductCommand>(model);
        await Mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category/{productId:guid}")]
    public async Task<IActionResult> Delete(Guid productId)
    {
        await Mediator.Send(
            new DeleteProductCommand()
            {
                ProductId = productId
            });
        
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/minimized-products")]
    public async Task<ActionResult> GetAll()
    {
        var result = 
            await Mediator.Send(
                new GetMiniProductListQuery());
        
        if(!result.Any())
            return NotFound();
        
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product/{id:guid}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = 
            await Mediator.Send(
                new GetProductByIdQuery
                {
                    Id = id
                });
        
        if(result is null)
            return NotFound();
        
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product/{article:int}")]
    public async Task<ActionResult> GetByArticle(int article)
    {
        var query = 
            await Mediator.Send(
                new GetProductByArticleQuery()
                {
                    Article = article
                });
        
        if(query is null)
            return NotFound();
        
        return Ok(query);
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product/{maxPrice:decimal}/{minPrice:decimal}/{descending:bool}")]
    public async Task<ActionResult> GetSortedByPrice(decimal? maxPrice = null, decimal? minPrice = null, bool descending = false)
    {
        var result = 
            await Mediator.Send(
                new GetSortedProductsByPriceQuery()
                {
                    MaxPrice = maxPrice,
                    MinPrice = minPrice,
                    Descending = descending
                });

        if (!result.Any())
            return BadRequest();
        
        return Ok(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/by-rating-product/{descending:bool}")]
    public async Task<ActionResult> GetSortedByRating(bool descending = false)
    {
        var result = 
            await Mediator.Send(
                new GetSortedProductsByRatingQuery()
                {
                    Descending = descending
                });

        if (!result.Any())
            return BadRequest();
        
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/search-products/{words}")]
    public async Task<ActionResult> SearchByName(string words)
    {
        var result = 
            await Mediator.Send(
                new GetProductByNameQuery()
                {
                    Key = words
                });
        
        if (!result.Any())
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/by-novelty-products/{descending:bool}")]
    public async Task<ActionResult> GetByNovelty(bool descending = true)
    {
        var result = 
            await Mediator.Send(
                new GetProductByNoveltyQuery()
                {
                    Descending = descending
                });
        
        if(!result.Any())
            return NotFound();
        
        return Ok(result);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/seller-products/{sellerId:int}")]
    public async Task<ActionResult> GetBySellerId(int sellerId)
    {
        var result = 
            await Mediator.Send(
                new GetProductBySellerIdQuery()
                {
                    SellerId = sellerId
                });
        
        if(!result.Any())
            return NotFound();
        
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/category-products/{name}")]
    public async Task<ActionResult> GetByCategoryName(string name)
    {
        return Ok();
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/featured-products")]
    public async Task<ActionResult> GetByFeatured()
    {
        return Ok();
    }
    
    [HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/discounts-products")]
    public async Task<ActionResult> GetDiscountsProducts()
    {
        return Ok();
    }
}*/