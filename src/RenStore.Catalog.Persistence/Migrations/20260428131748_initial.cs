using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RenStore.Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "product_article_seq",
                startValue: 1000L);

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
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
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
                name: "colors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    normalized_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name_ru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    normalized_name_ru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    color_code = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    kind = table.Column<int>(type: "integer", nullable: false),
                    occurred_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    processed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    error = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "price_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
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
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<int>(type: "integer", nullable: false),
                    discount_percents = table.Column<int>(type: "integer", maxLength: 3, nullable: true),
                    is_verified_seller = table.Column<bool>(type: "boolean", nullable: true),
                    in_stock_overall = table.Column<int>(type: "integer", nullable: true),
                    sales_count_overall = table.Column<int>(type: "integer", nullable: true),
                    reviews_count = table.Column<int>(type: "integer", nullable: true),
                    average_rating = table.Column<double>(type: "double precision", nullable: true)
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
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
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
                    type_of_packing = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
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
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
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
                    updated_by_id = table.Column<Guid>(type: "uuid", nullable: true),
                    updated_by_role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    in_stock = table.Column<int>(type: "integer", nullable: true),
                    sales_count = table.Column<int>(type: "integer", nullable: true),
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
                name: "IX_colors_name_ru",
                table: "colors",
                column: "name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_normalized_name",
                table: "colors",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_colors_color_code",
                table: "colors",
                column: "color_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_aggregate_id",
                table: "outbox_messages",
                column: "aggregate_id");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_unprocessed",
                table: "outbox_messages",
                columns: new[] { "processed_at", "created_at" },
                filter: "processed_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "ux_price_history_size_id_price",
                table: "price_history",
                columns: new[] { "size_id", "price" });

            migrationBuilder.CreateIndex(
                name: "idx_average_rating",
                table: "product_variants",
                column: "average_rating",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "idx_color_id",
                table: "product_variants",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "idx_sales_count",
                table: "product_variants",
                column: "sales_count_overall",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "idx_variant_article",
                table: "product_variants",
                column: "article",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_variant_normalized_name",
                table: "product_variants",
                column: "normalized_name");

            migrationBuilder.CreateIndex(
                name: "idx_variant_url",
                table: "product_variants",
                column: "url",
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
                name: "ux_variant_images_lookup",
                table: "variant_images",
                columns: new[] { "variant_id", "is_deleted", "is_main" });

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
                name: "colors");

            migrationBuilder.DropTable(
                name: "outbox_messages");

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

            migrationBuilder.DropSequence(
                name: "product_article_seq");
        }
    }
}
