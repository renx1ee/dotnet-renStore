global using MediatR;
global using Microsoft.Extensions.Logging;
global using FluentValidation;

global using RenStore.Inventory.Domain.Aggregates.Stock;
global using RenStore.Inventory.Domain.Interfaces.Repository;
global using RenStore.SharedKernal.Domain.Exceptions;
global using RenStore.Inventory.Domain.Constants;
global using RenStore.Inventory.Application.Abstractions;
global using RenStore.Inventory.Application.Abstractions.Projections;
global using RenStore.Inventory.Application.ReadModels;