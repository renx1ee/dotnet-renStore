# Sample ASP.NET Core Store

## Docker Configuration

### Start a postgres instance
```
docker run --name my_postgres -p 5438:5432 -e POSTGRES_PASSWORD=postgres -d postgres
```
[Oficial Documentation Docker](https://hub.docker.com/_/postgres)

## Redis Configuration
```
docker run --name my-redis -p 6379:6379 -d redis
```

## Dotnet Configuration

### Installing the tools
```
dotnet tool install --global dotnet-ef
```

```
dotnet tool update --global dotnet-ef
```

[Oficial Documentation Microsoft](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

### Using the tools

#### Adding the first catalog migration
```
 dotnet ef migrations add initial -c CatalogDbContext --project ./RenStore.Catalog.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Catalog.WebApi/RenStore.Catalog.WebApi.csproj"
```

#### Catalog Migration update
```
dotnet ef database update -c CatalogDbContext --project ./RenStore.Catalog.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Catalog.WebApi/RenStore.Catalog.WebApi.csproj"       
```

#### Adding the first inventory migration
```
dotnet ef migrations add initial -c InventoryDbContext --project ./RenStore.Inventory.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Inventory.WebApi/RenStore.Inventory.WebApi.csproj"
```

#### Inventory Migration update
```
dotnet ef database update -c InventoryDbContext --project ./RenStore.Inventory.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Inventory.WebApi/RenStore.Inventory.WebApi.csproj"
```

#### Adding the first ordering migration 
```
 dotnet ef migrations add Initial -c OrderingDbContext --project ./RenStore.Ordering.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Ordering.WebApi/RenStore.Ordering.WebApi.csproj"
```

#### Ordering Migration update
```
 dotnet ef database update -c OrderingDbContext --project ./RenStore.Ordering.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Ordering.WebApi/RenStore.Ordering.WebApi.csproj"
```

#### Adding the first ordering SAGA migration
```
 dotnet ef migrations add Initial -c OrderSagaDbContext --project ./RenStore.Ordering.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Ordering.WebApi/RenStore.Ordering.WebApi.csproj"
```

#### Ordering SAGA Migration update
```
dotnet ef database update Initial -c OrderSagaDbContext --project ./RenStore.Ordering.Persistence --startup-project "/Users/re/Documents/Projects/C#/RenStore/src/RenStore.Ordering.WebApi/RenStore.Ordering.WebApi.csproj"
```

