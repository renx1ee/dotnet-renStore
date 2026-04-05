using System.Data;
using Dapper;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations.SqlMappers;

public class ProductStatusHandler : SqlMapper.TypeHandler<ProductStatus>
{
    public override void SetValue(IDbDataParameter parameter, ProductStatus value)
    {
        parameter.Value = ProductStatusConversion.ToDatabase(value);
    }

    public override ProductStatus Parse(object value)
    {
        return ProductStatusConversion.FromDatabase(value.ToString());
    }
}