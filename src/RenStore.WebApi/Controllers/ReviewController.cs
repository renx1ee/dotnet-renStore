/*using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RenStore.Application.Dto.Review;
using RenStore.Application.Features.Review.Commands.Create;
using RenStore.Application.Features.Review.Commands.Delete;
using RenStore.Application.Features.Review.Commands.Edit;
using RenStore.Application.Features.Review.Queries.GetAllByProductId;
using RenStore.Application.Features.Review.Queries.GetAllReviews;
using RenStore.Application.Features.Review.Queries.GetAllReviewsByUserId;
using RenStore.Application.Features.Review.Queries.GetFirstByCreatedDate;
using RenStore.Application.Features.Review.Queries.GetFirstByRating;
using RenStore.Domain.Enums;

namespace RenStore.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReviewController(IMapper mapper) : BaseController
{
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/review")]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto model)
    {
        var review = mapper.Map<CreateReviewCommand>(model);
        var result = await Mediator.Send(review);
        
        if(result != Guid.Empty)
            return Accepted(); 
        
        return BadRequest();
    }
    
    // TODO: доделать
    [HttpPatch]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/review/{id:guid}")]
    public async Task<IActionResult> Edit(Guid id, [FromBody] UpdateReviewDto model)
    {
        var review = mapper.Map<UpdateReviewCommand>(model);
        await Mediator.Send(review);
        return NoContent();
    }

    [HttpDelete]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/review/{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteReviewCommand { Id = id });
        return NoContent();
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/reviews")]
    public async Task<IActionResult> GetAll([FromQuery] ReviewStatusFilter status = ReviewStatusFilter.All)
    {
        var result = await Mediator.Send(
            new GetAllReviewsQuery()
            {
                Status = status
            });
        
        if (!result.Any())
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-reviews/{productId:guid}")]
    public async Task<IActionResult> GetByProductId(Guid productId)
    {
        var result = await Mediator.Send(
            new GetAllReviewsByProductIdQuery()
            {
                ProductId = productId
            });

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }
    
    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/user-reviews/{userId:guid}")]
    public async Task<IActionResult> GetByUserId([FromQuery] ReviewStatusFilter status, string userId)
    {
        var result = await Mediator.Send(
        new GetAllReviewsByUserIdQuery()
        {
            UserId = userId,
            Status = status
        });

        if (!result.Any())
            return NotFound();

        return Ok(result);
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-reviews-by-rating/{productId:guid}/{count:int}")]
    public async Task<IActionResult> GetFirstByRating(Guid productId, int count)
    {
        var request = await Mediator.Send(
            new GetFirstReviewsByRatingQuery()
            {
                ProductId = productId,
                Count = count
            });

        if(!request.Any())
            return NotFound();

        return Ok(request);
    }

    [HttpGet]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/product-reviews-by-date/{productId:guid}/{count:int}")]
    public async Task<IActionResult> GetFirstByDate(Guid productId, int count)
    {
        var request = await Mediator.Send(
            new GetFirstReviewsByDateQuery()
            {
                ProductId = productId,
                Count = count
            });

        if(!request.Any())
            return NotFound();

        return Ok(request);
    }
    // TODO: переделать
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/like-review/{reviewId:guid}/{userId}")]
    public async Task<IActionResult> LikeReview(Guid reviewId, string userId)
    {
        // TODO: доделать лайк отзыва
        return Ok();
    }
    // TODO: переделать
    [HttpPost]
    [MapToApiVersion(1)]
    [Route("/api/v{version:apiVersion}/ulike-review/{reviewId:guid}/{userId}")]
    public async Task<IActionResult> UnlikeReview()
    {
        // TODO: доделать снятие лайка с отзыва
        return Ok();
    }
}*/