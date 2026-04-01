global using RenStore.Catalog.Contracts.Enums.Sorting;
global using RenStore.Catalog.WebApi.Extensions;
global using Asp.Versioning;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;

global using RenStore.Catalog.WebApi.Requests.Variant; 

global using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindPriceHistoryBySizeId;
global using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindSizesByVariantId;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.PublishVariant;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.SetMainImageId;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;
global using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByArticle;
global using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindById;
global using RenStore.Catalog.Application.Features.ProductVariant.Queries.FindByProductId;
global using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;
global using RenStore.SharedKernal.Domain.Constants;

global using RenStore.Catalog.Application.Features.Product.Commands.Approve;
global using RenStore.Catalog.Application.Features.Product.Commands.Archive;
global using RenStore.Catalog.Application.Features.Product.Commands.Create;
global using RenStore.Catalog.Application.Features.Product.Commands.Hide;
global using RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;
global using RenStore.Catalog.Application.Features.Product.Commands.Reject;
global using RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;
global using RenStore.Catalog.Application.Features.Product.Commands.ToDraft;
global using RenStore.Catalog.Application.Features.Product.Queries.FindById;
global using RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;

global using RenStore.Catalog.Application.Features.Category.Commands.Activate;
global using RenStore.Catalog.Application.Features.Category.Commands.ActivateSubCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.Create;
global using RenStore.Catalog.Application.Features.Category.Commands.CreateSubCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.Deactivate;
global using RenStore.Catalog.Application.Features.Category.Commands.DeactivateSubCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.Restore;
global using RenStore.Catalog.Application.Features.Category.Commands.RestoreSubCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.SoftDelete;
global using RenStore.Catalog.Application.Features.Category.Commands.SoftDeleteSubCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.UpdateCategory;
global using RenStore.Catalog.Application.Features.Category.Commands.UpdateSubCategory;
global using RenStore.Catalog.WebApi.Requests.Category;

global using RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;
global using RenStore.Catalog.Application.Features.VariantMedia.Commands.Upload;