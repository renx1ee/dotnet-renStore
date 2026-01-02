using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RenStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<double>(type: "double precision", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_category_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    category_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_category_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    category_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 642, DateTimeKind.Utc).AddTicks(5110))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    color_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    color_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    normalized_color_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    color_name_ru = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    color_code = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: true),
                    color_description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("color_id", x => x.color_id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    normalized_country_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    other_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    normalized_other_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    country_name_ru = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    normalized_country_name_ru = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    country_code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    country_phone_code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "sellers",
                columns: table => new
                {
                    seller_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    seller_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    normalized_seller_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    seller_description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 632, DateTimeKind.Utc).AddTicks(1290)),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sellers", x => x.seller_id);
                    table.ForeignKey(
                        name: "FK_sellers_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_carts",
                columns: table => new
                {
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_price = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 662, DateTimeKind.Utc).AddTicks(8470)),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_carts", x => x.cart_id);
                    table.ForeignKey(
                        name: "FK_shopping_carts_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_images",
                columns: table => new
                {
                    user_image_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_file_name = table.Column<string>(type: "text", nullable: false),
                    storage_path = table.Column<string>(type: "text", nullable: false),
                    file_size_bites = table.Column<long>(type: "bigint", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sort_order = table.Column<short>(type: "smallint", nullable: false),
                    uploaded_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 677, DateTimeKind.Utc).AddTicks(1940)),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_images", x => x.user_image_id);
                    table.ForeignKey(
                        name: "FK_user_images_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sub_categories",
                columns: table => new
                {
                    sub_category_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sub_category_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_sub_category_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    sub_category_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_sub_category_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    sub_category_description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sub_categories", x => x.sub_category_id);
                    table.ForeignKey(
                        name: "FK_sub_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    city_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    city_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_city_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_city_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cities", x => x.city_id);
                    table.ForeignKey(
                        name: "FK_cities_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    overall_rating = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    seller_id = table.Column<long>(type: "bigint", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.product_id);
                    table.ForeignKey(
                        name: "category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "seller_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_complains",
                columns: table => new
                {
                    seller_complain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    custom_reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 688, DateTimeKind.Utc).AddTicks(2280)),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderator_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    moderator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    seller_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_complains", x => x.seller_complain_id);
                    table.ForeignKey(
                        name: "FK_seller_complains_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_seller_complains_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "seller_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_images",
                columns: table => new
                {
                    seller_image_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_file_name = table.Column<string>(type: "text", nullable: false),
                    storage_path = table.Column<string>(type: "text", nullable: false),
                    file_size_bites = table.Column<long>(type: "bigint", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sort_order = table.Column<short>(type: "smallint", nullable: false),
                    uploaded_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 675, DateTimeKind.Utc).AddTicks(8220)),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    seller_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_images", x => x.seller_image_id);
                    table.ForeignKey(
                        name: "FK_seller_images_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "seller_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HouseCode = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    BuildingNumber = table.Column<string>(type: "text", nullable: false),
                    ApartmentNumber = table.Column<string>(type: "text", nullable: false),
                    Entrance = table.Column<string>(type: "text", nullable: false),
                    Floor = table.Column<long>(type: "bigint", nullable: false),
                    FlatNumber = table.Column<string>(type: "text", nullable: false),
                    FullAddress = table.Column<string>(type: "text", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: false),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_cities_CityId",
                        column: x => x.CityId,
                        principalTable: "cities",
                        principalColumn: "city_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                columns: table => new
                {
                    cart_item_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", maxLength: 5, nullable: false, defaultValue: 1),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => x.cart_item_id);
                    table.ForeignKey(
                        name: "FK_cart_items_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_cart_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_items_shopping_carts_cart_id",
                        column: x => x.cart_id,
                        principalTable: "shopping_carts",
                        principalColumn: "cart_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_clothes",
                columns: table => new
                {
                    product_cloth_id = table.Column<Guid>(type: "uuid", nullable: false),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    season = table.Column<int>(type: "integer", nullable: true),
                    neckline = table.Column<int>(type: "integer", nullable: true),
                    the_cut = table.Column<int>(type: "integer", nullable: true),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_clothes", x => x.product_cloth_id);
                    table.ForeignKey(
                        name: "product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    normalized_variant_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    rating = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    article = table.Column<long>(type: "bigint", nullable: false),
                    in_stock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 652, DateTimeKind.Utc).AddTicks(8490)),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    color_id = table.Column<int>(type: "integer", nullable: false),
                    ProductDetailId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComplainId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.product_variant_id);
                    table.ForeignKey(
                        name: "FK_product_variants_colors_color_id",
                        column: x => x.color_id,
                        principalTable: "colors",
                        principalColumn: "color_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_cloth_sizes",
                columns: table => new
                {
                    cloth_size_id = table.Column<Guid>(type: "uuid", nullable: false),
                    cloth_size = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false),
                    product_cloth_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_cloth_sizes", x => x.cloth_size_id);
                    table.ForeignKey(
                        name: "product_cloth_id",
                        column: x => x.product_cloth_id,
                        principalTable: "product_clothes",
                        principalColumn: "product_cloth_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_attributes",
                columns: table => new
                {
                    attribute_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attribute_name = table.Column<string>(type: "text", nullable: false),
                    attribute_value = table.Column<string>(type: "text", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_attributes", x => x.attribute_id);
                    table.ForeignKey(
                        name: "product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_details",
                columns: table => new
                {
                    product_detail_id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(2500)", maxLength: 2500, nullable: false),
                    model_features = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    decorative_elements = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    equipment = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    composition = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    caring_of_things = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type_of_packing = table.Column<int>(type: "integer", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_details", x => x.product_detail_id);
                    table.ForeignKey(
                        name: "country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                columns: table => new
                {
                    product_image_id = table.Column<Guid>(type: "uuid", nullable: false),
                    original_file_name = table.Column<string>(type: "text", nullable: false),
                    storage_path = table.Column<string>(type: "text", nullable: false),
                    file_size_bites = table.Column<long>(type: "bigint", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    sort_order = table.Column<short>(type: "smallint", nullable: false),
                    uploaded_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 674, DateTimeKind.Utc).AddTicks(4170)),
                    weight = table.Column<int>(type: "integer", nullable: false),
                    height = table.Column<int>(type: "integer", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.product_image_id);
                    table.ForeignKey(
                        name: "FK_product_images_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_price_histories",
                columns: table => new
                {
                    price_history_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    old_price = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_price = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    changed_by = table.Column<string>(type: "text", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_price_histories", x => x.price_history_id);
                    table.ForeignKey(
                        name: "product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_questions",
                columns: table => new
                {
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 671, DateTimeKind.Utc).AddTicks(1540)),
                    ModeratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_approved = table.Column<bool>(type: "boolean", nullable: true),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ComplainId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_questions", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_product_questions_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_questions_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_variant_complains",
                columns: table => new
                {
                    product_variant_complain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    custom_reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 684, DateTimeKind.Utc).AddTicks(90)),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderator_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    moderator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variant_complains", x => x.product_variant_complain_id);
                    table.ForeignKey(
                        name: "FK_product_variant_complains_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_variant_complains_product_variants_product_variant_~",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    review_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    rating = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    status = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    product_variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 668, DateTimeKind.Utc).AddTicks(6790)),
                    is_approved = table.Column<bool>(type: "boolean", nullable: false),
                    is_updated = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    last_updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviews", x => x.review_id);
                    table.ForeignKey(
                        name: "FK_reviews_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviews_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "product_variant_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_answers",
                columns: table => new
                {
                    answer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 672, DateTimeKind.Utc).AddTicks(5730)),
                    ModeratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_approved = table.Column<bool>(type: "boolean", nullable: true),
                    seller_id = table.Column<long>(type: "bigint", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_answers", x => x.answer_id);
                    table.ForeignKey(
                        name: "FK_product_answers_product_questions_question_id",
                        column: x => x.question_id,
                        principalTable: "product_questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_answers_sellers_seller_id",
                        column: x => x.seller_id,
                        principalTable: "sellers",
                        principalColumn: "seller_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question_complains",
                columns: table => new
                {
                    question_complain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    custom_reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 681, DateTimeKind.Utc).AddTicks(2240)),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderator_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    moderator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_complains", x => x.question_complain_id);
                    table.ForeignKey(
                        name: "FK_question_complains_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_question_complains_product_questions_product_question_id",
                        column: x => x.product_question_id,
                        principalTable: "product_questions",
                        principalColumn: "question_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review_complains",
                columns: table => new
                {
                    review_complain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    custom_reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 686, DateTimeKind.Utc).AddTicks(1490)),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderator_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    moderator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    review_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review_complains", x => x.review_complain_id);
                    table.ForeignKey(
                        name: "FK_review_complains_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_review_complains_reviews_review_id",
                        column: x => x.review_id,
                        principalTable: "reviews",
                        principalColumn: "review_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answer_complains",
                columns: table => new
                {
                    answer_complain_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<int>(type: "integer", nullable: false),
                    custom_reason = table.Column<string>(type: "text", nullable: true),
                    comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValue: new DateTime(2025, 12, 27, 3, 5, 32, 678, DateTimeKind.Utc).AddTicks(9900)),
                    status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    resolved_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    moderator_comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    moderator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    product_answer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answer_complains", x => x.answer_complain_id);
                    table.ForeignKey(
                        name: "FK_answer_complains_AspNetUsers_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_answer_complains_product_answers_product_answer_id",
                        column: x => x.product_answer_id,
                        principalTable: "product_answers",
                        principalColumn: "answer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ApplicationUserId",
                table: "Addresses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_answer_complains_product_answer_id",
                table: "answer_complains",
                column: "product_answer_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_answer_complains_user_id",
                table: "answer_complains",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_ApplicationUserId",
                table: "cart_items",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_cart_id",
                table: "cart_items",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_product_id",
                table: "cart_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_categories_normalized_category_name",
                table: "categories",
                column: "normalized_category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categories_normalized_category_name_ru",
                table: "categories",
                column: "normalized_category_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cities_country_id",
                table: "cities",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_colors_color_name",
                table: "colors",
                column: "color_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_color_name_ru",
                table: "colors",
                column: "color_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_normalized_color_name",
                table: "colors",
                column: "normalized_color_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_country_code",
                table: "countries",
                column: "country_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_normalized_country_name",
                table: "countries",
                column: "normalized_country_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_countries_normalized_country_name_ru",
                table: "countries",
                column: "normalized_country_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_answers_question_id",
                table: "product_answers",
                column: "question_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_answers_seller_id",
                table: "product_answers",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_attributes_product_variant_id",
                table: "product_attributes",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_cloth_sizes_product_cloth_id",
                table: "product_cloth_sizes",
                column: "product_cloth_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_clothes_product_id",
                table: "product_clothes",
                column: "product_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_details_country_id",
                table: "product_details",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_details_product_variant_id",
                table: "product_details",
                column: "product_variant_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_images_product_variant_id",
                table: "product_images",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_price_histories_product_variant_id",
                table: "product_price_histories",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_questions_product_variant_id",
                table: "product_questions",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_questions_user_id",
                table: "product_questions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_complains_product_variant_id",
                table: "product_variant_complains",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_complains_user_id",
                table: "product_variant_complains",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_article",
                table: "product_variants",
                column: "article",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_color_id",
                table: "product_variants",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_normalized_variant_name",
                table: "product_variants",
                column: "normalized_variant_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_seller_id",
                table: "products",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_complains_product_question_id",
                table: "question_complains",
                column: "product_question_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_question_complains_user_id",
                table: "question_complains",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_complains_review_id",
                table: "review_complains",
                column: "review_id");

            migrationBuilder.CreateIndex(
                name: "IX_review_complains_user_id",
                table: "review_complains",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_product_variant_id",
                table: "reviews",
                column: "product_variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_user_id",
                table: "reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_seller_complains_seller_id",
                table: "seller_complains",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_seller_complains_user_id",
                table: "seller_complains",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_seller_images_seller_id",
                table: "seller_images",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_sellers_normalized_seller_name",
                table: "sellers",
                column: "normalized_seller_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sellers_seller_name",
                table: "sellers",
                column: "seller_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sellers_user_id",
                table: "sellers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shopping_carts_user_id",
                table: "shopping_carts",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_category_id",
                table: "sub_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_normalized_sub_category_name",
                table: "sub_categories",
                column: "normalized_sub_category_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_sub_categories_normalized_sub_category_name_ru",
                table: "sub_categories",
                column: "normalized_sub_category_name_ru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_images_user_id",
                table: "user_images",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "answer_complains");

            migrationBuilder.DropTable(
                name: "cart_items");

            migrationBuilder.DropTable(
                name: "product_attributes");

            migrationBuilder.DropTable(
                name: "product_cloth_sizes");

            migrationBuilder.DropTable(
                name: "product_details");

            migrationBuilder.DropTable(
                name: "product_images");

            migrationBuilder.DropTable(
                name: "product_price_histories");

            migrationBuilder.DropTable(
                name: "product_variant_complains");

            migrationBuilder.DropTable(
                name: "question_complains");

            migrationBuilder.DropTable(
                name: "review_complains");

            migrationBuilder.DropTable(
                name: "seller_complains");

            migrationBuilder.DropTable(
                name: "seller_images");

            migrationBuilder.DropTable(
                name: "sub_categories");

            migrationBuilder.DropTable(
                name: "user_images");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "product_answers");

            migrationBuilder.DropTable(
                name: "shopping_carts");

            migrationBuilder.DropTable(
                name: "product_clothes");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "product_questions");

            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "sellers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
