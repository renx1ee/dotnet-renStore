/*using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using RenStore.Persistence.Features.Review.Commands.Create;
using RenStore.Persistence.Features.Review.Commands.Delete;
using RenStore.Persistence.Features.Review.Commands.Moderate;
using RenStore.Persistence.Features.Review.Commands.Edit;

namespace RenStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        // Product
        services.AddAutoMapper(typeof(CreateClothesProductMappingProfile));
        services.AddAutoMapper(typeof(CreateShoesProducMappingProfile));
        services.AddAutoMapper(typeof(UpdateProductMappingProfile));
        services.AddAutoMapper(typeof(GetProductMappingProfile));
        services.AddAutoMapper(typeof(GetProductByArticleMappingProfile));
        // CategoryEntity
        services.AddAutoMapper(typeof(CreateCategoryMappingModel));
        services.AddAutoMapper(typeof(UpdateCategoryMappingProfile));
        services.AddAutoMapper(typeof(GetCategoryByIdMappingProfile));
        services.AddAutoMapper(typeof(GetCategoryByNameMappingProfile));
        // SellerEntity
        services.AddAutoMapper(typeof(UpdateSellerMappingProfile));
        services.AddAutoMapper(typeof(CreateSellerMappingProfile));
        services.AddAutoMapper(typeof(GetAllSellersMappingProfile));
        services.AddAutoMapper(typeof(GetSellerByNameMappingProfile));
        // Review
        services.AddAutoMapper(typeof(CreateReviewMappingProfile));
        services.AddAutoMapper(typeof(UpdateReviewMappingProfile));
        // Order
        services.AddAutoMapper(typeof(CreateOrderMappingProfile));
        services.AddAutoMapper(typeof(UpdateOrderMappingProfile));
        services.AddAutoMapper(typeof(UpdateOrderMappingProfile));
        services.AddAutoMapper(typeof(GetOrderByIdMappingProfile));
        // Shopping Cart
        services.AddAutoMapper(typeof(AddToCartMappingProfile));
        // Question
        services.AddAutoMapper(typeof(GetQuestionByIdMappingProfile));
        services.AddAutoMapper(typeof(GetQuestionWithAnswerByIdMappingProfile));
        // Answer
        services.AddAutoMapper(typeof(GetAnswerMappingProfile));
        
        services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
        
        // Product
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateClothesProductCommand).Assembly,
                typeof(CreateShoesProductCommand).Assembly,
                typeof(UpdateProductCommand).Assembly,
                typeof(DeleteProductCommand).Assembly,
                typeof(GetProductByIdQuery).Assembly,
                typeof(GetMiniProductListQuery).Assembly,
                typeof(GetProductByArticleQuery).Assembly,
                typeof(GetSortedProductsByPriceQuery).Assembly,
                typeof(GetSortedProductsByRatingQuery).Assembly,
                typeof(GetProductBySellerIdQuery).Assembly,
                typeof(GetProductByNoveltyQuery).Assembly,
                typeof(GetProductByNameQuery).Assembly,
                typeof(CreateClothesProductCommandHandler).Assembly,
                typeof(CreateShoesProductCommandHandler).Assembly,
                typeof(UpdateProductCommandHandler).Assembly,
                typeof(GetProductByIdQueryHandler).Assembly,
                typeof(GetMiniProductListQueryHandler).Assembly,
                typeof(GetProductByArticleQueryHandler).Assembly,
                typeof(GetSortedProductsByPriceQueryHandler).Assembly,
                typeof(GetSortedProductsByRatingQueryHandler).Assembly,
                typeof(GetProductBySellerIdQueryHandler).Assembly,
                typeof(GetProductByNoveltyQueryHandler).Assembly
                ));
        // CategoryEntity
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateCategoryCommand).Assembly,
                typeof(UpdateCategoryCommand).Assembly,
                typeof(DeleteCategoryCommand).Assembly,
                typeof(GetCategoriesListQuery).Assembly,
                typeof(GetCategoryByIdQuery).Assembly,
                typeof(GetCategoryByNameQuery).Assembly,
                typeof(CreateCategoryCommandHandler).Assembly,
                typeof(UpdateCategoryCommandHandler).Assembly,
                typeof(DeleteCategoryCommandHandler).Assembly,
                typeof(GetAllCategoriesQueryHandler).Assembly,
                typeof(GetCategoryByIdQueryHandler).Assembly,
                typeof(GetCategoryByNameQueryHandler).Assembly
                ));
        // SellerEntity
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateSellerCommand).Assembly,
                typeof(UpdateSellerCommand).Assembly,
                typeof(DeleteSellerCommand).Assembly,
                typeof(GetAllSellersListQuery).Assembly,
                typeof(GetSellerByIdQuery).Assembly,
                typeof(GetSellerByNameQuery).Assembly,
                typeof(CreateSellerCommandHandler).Assembly,
                typeof(UpdateSellerCommandHandler).Assembly,
                typeof(DeleteSellerCommandHandler).Assembly,
                typeof(GetAllSellersQueryHandler).Assembly,
                typeof(GetCategoryByIdQueryHandler).Assembly,
                typeof(GetSellerByNameQueryHandler).Assembly
                ));
        // Review
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateReviewCommand).Assembly,
                typeof(UpdateReviewCommand).Assembly,
                typeof(DeleteReviewCommand).Assembly,
                typeof(ModerateReviewCommand).Assembly,
                typeof(GetAllReviewsQuery).Assembly,
                typeof(GetAllReviewsByProductIdQuery).Assembly,
                typeof(GetAllReviewsByUserIdQuery).Assembly,
                typeof(GetFirstReviewsByDateQuery).Assembly,
                typeof(GetFirstReviewsByRatingQuery).Assembly,
                typeof(GetAllForModerationRequest).Assembly,
                typeof(CreateReviewCommandHandler).Assembly,
                typeof(UpdateReviewCommandHandler).Assembly,
                typeof(DeleteReviewCommandHandler).Assembly,
                typeof(ModerateReviewCommandHandler).Assembly,
                typeof(GetAllCategoriesQueryHandler).Assembly,
                typeof(GetAllReviewsByProductIdQueryHandler).Assembly,
                typeof(GetAllReviewsByUserIdQueryHandler).Assembly,
                typeof(GetAllForModerationRequestHandler).Assembly
                ));
        // Order
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateOrderCommand).Assembly,
                typeof(UpdateOrderCommand).Assembly,
                typeof(DeleteOrderCommand).Assembly,
                typeof(GetAllOrdersQuery).Assembly,
                typeof(GetOrderByIdQuery).Assembly,
                typeof(GetOrdersByProductIdQuery).Assembly,
                typeof(GetOrdersByUserIdQuery).Assembly,
                typeof(CreateOrderCommandHandler).Assembly,
                typeof(UpdateOrderCommandHandler).Assembly,
                typeof(DeleteOrderCommandHandler).Assembly,
                typeof(GetAllCategoriesQueryHandler).Assembly,
                typeof(GetOrderByIdQueryHandler).Assembly,
                typeof(GetOrdersByProductIdQueryHandler).Assembly,
                typeof(GetOrdersByUserIdQueryHandler).Assembly
                ));
        // Shopping Cart
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(AddToCartCommand).Assembly,
                typeof(RemoveFromCartCommand).Assembly,
                typeof(ClearCartCommand).Assembly,
                typeof(GetShoppingCartItemsByUserIdQuery).Assembly,
                typeof(GetAllCartItemsQuery).Assembly,
                typeof(GetTotalShoppingCartPriceQuery).Assembly,
                typeof(AddToCartCommandHandler).Assembly,
                typeof(RemoveFromCartCommandHandler).Assembly,
                typeof(ClearCartCommandHandler).Assembly,
                typeof(GetShoppingCartItemsByUserIdQueryHandler).Assembly,
                typeof(GetTotalShoppingCartPriceQueryHandler).Assembly,
                typeof(GetAllCartItemsQueryHandler).Assembly,
                typeof(GetTotalShoppingCartPriceQueryHandler).Assembly
                ));
        // Question
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateProductQuestionCommand).Assembly,
                typeof(DeleteProductQuestionCommand).Assembly,
                typeof(GetAllQuestionsQuery).Assembly,
                typeof(GetProductQuestionByIdQuery).Assembly,
                typeof(GetQuestionWithAnswerByIdQuery).Assembly,
                typeof(GetAllQuestionsByUserIdQuery).Assembly,
                typeof(GetAllQuestionsByProductIdQuery).Assembly,
                typeof(GetAllQuestionsWithAnswersQuery).Assembly,
                typeof(CreateProductQuestionCommandHandler).Assembly,
                typeof(DeleteProductQuestionCommandHandler).Assembly,
                typeof(GetAllQuestionsCommandHandler).Assembly,
                typeof(GetProductQuestionByIdQueryHandler).Assembly,
                typeof(GetQuestionWithAnswerByIdQueryHandler).Assembly,
                typeof(GetAllQuestionsByUserIdQueryHandler).Assembly,
                typeof(GetAllQuestionsByProductIdQueryHandler).Assembly
            ));
        // Answer
        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(
                typeof(CreateProductAnswerCommand).Assembly,
                typeof(DeleteProductAnswerCommand).Assembly,
                typeof(GetAnswerByIdQuery).Assembly,
                typeof(CreateProductAnswerCommandHandler).Assembly,
                typeof(DeleteProductAnswerCommandHandler).Assembly,
                typeof(GetAnswerByIdQueryHandler).Assembly
                ));

        services.AddMediatR(config => config.RegisterServicesFromAssembly(
            Assembly.GetExecutingAssembly()));
        
        return services;
    }
}*/