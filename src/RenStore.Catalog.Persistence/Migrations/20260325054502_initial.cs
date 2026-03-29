using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RenStore.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "catalog_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_type = table.Column<string>(type: "text", nullable: false),
                    version = table.Column<int>(type: "integer", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: true),
                    occurred_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_catalog_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    category_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "price_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<int>(type: "integer", nullable: false),
                    valid_from = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deactivated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    size_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_price_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    article = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    main_image_id = table.Column<Guid>(type: "uuid", nullable: false),
                    size_system = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    size_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sub_category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sub_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variant_attributes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_attributes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variant_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    composition = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    model_features = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    decorative_elements = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    equipment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    caring_of_things = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    type_of_packing = table.Column<int>(type: "integer", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedByRole = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variant_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_file_name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    storage_path = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    file_size_bites = table.Column<long>(type: "bigint", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    uploaded_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "variant_sizes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    letter_size = table.Column<int>(type: "integer", nullable: false),
                    size_number = table.Column<decimal>(type: "numeric", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    size_system = table.Column<int>(type: "integer", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variant_sizes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ux_events_aggregate_id_version",
                table: "catalog_events",
                columns: new[] { "aggregate_id", "version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_normalized_name",
                table: "categories",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_normalized_name_ru",
                table: "categories",
                column: "normalized_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_price_history_size_id_price",
                table: "price_history",
                columns: new[] { "size_id", "price" });

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_article",
                table: "product_variants",
                column: "article",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_normalized_name",
                table: "product_variants",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_normalized_name",
                table: "sub_categories",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_normalized_name_ru",
                table: "sub_categories",
                column: "normalized_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_variant_attributes_key",
                table: "variant_attributes",
                column: "key");

            migrationBuilder.CreateIndex(
                name: "ux_variant_attributes_variant_id",
                table: "variant_attributes",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "ux_variant_images_variant_id_is_main",
                table: "variant_images",
                columns: new[] { "variant_id", "is_main" });

            migrationBuilder.CreateIndex(
                name: "ux_variant_sizes_variant_id",
                table: "variant_sizes",
                column: "variant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "catalog_events");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "price_history");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "sub_categories");

            migrationBuilder.DropTable(
                name: "variant_attributes");

            migrationBuilder.DropTable(
                name: "variant_details");

            migrationBuilder.DropTable(
                name: "variant_images");

            migrationBuilder.DropTable(
                name: "variant_sizes");
        }
    }
}
