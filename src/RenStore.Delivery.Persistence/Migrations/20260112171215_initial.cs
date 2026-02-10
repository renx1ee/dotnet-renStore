using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RenStore.Delivery.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    country_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    normalized_country_name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    country_name_ru = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    normalized_country_name_ru = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    country_code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false),
                    country_phone_code = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.country_id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_tariffs",
                columns: table => new
                {
                    delivery_tariff_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    weight_limit_kg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true),
                    is_deleted = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_tariffs", x => x.delivery_tariff_id);
                });

            migrationBuilder.CreateTable(
                name: "cities",
                columns: table => new
                {
                    city_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    city_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    city_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_city_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    normalized_city_name_ru = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    country_id = table.Column<int>(type: "int", nullable: false)
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
                name: "delivery_orders",
                columns: table => new
                {
                    delivery_order_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    delivered_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    delivery_tariff_id = table.Column<int>(type: "int", nullable: false),
                    current_sorting_center_id = table.Column<long>(type: "bigint", nullable: true),
                    destination_sorting_center_id = table.Column<long>(type: "bigint", nullable: true),
                    pickup_point_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_orders", x => x.delivery_order_id);
                    table.ForeignKey(
                        name: "FK_delivery_orders_delivery_tariffs_delivery_tariff_id",
                        column: x => x.delivery_tariff_id,
                        principalTable: "delivery_tariffs",
                        principalColumn: "delivery_tariff_id");
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    address_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    house_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    street = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    building_number = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: true),
                    apartment_number = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    entrance = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    floor = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    updated_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    country_id = table.Column<int>(type: "int", nullable: false),
                    city_id = table.Column<int>(type: "int", nullable: false),
                    full_address_ru = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    full_address_en = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.address_id);
                    table.ForeignKey(
                        name: "FK_addresses_cities_city_id",
                        column: x => x.city_id,
                        principalTable: "cities",
                        principalColumn: "city_id");
                    table.ForeignKey(
                        name: "FK_addresses_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "country_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pickup_points",
                columns: table => new
                {
                    pickup_point_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    delete_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pickup_points", x => x.pickup_point_id);
                    table.ForeignKey(
                        name: "FK_pickup_points_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id");
                });

            migrationBuilder.CreateTable(
                name: "sorting_centers",
                columns: table => new
                {
                    sorting_center_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    address_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    delete_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sorting_centers", x => x.sorting_center_id);
                    table.ForeignKey(
                        name: "FK_sorting_centers_addresses_address_id",
                        column: x => x.address_id,
                        principalTable: "addresses",
                        principalColumn: "address_id");
                });

            migrationBuilder.CreateTable(
                name: "delivery_tracking_history",
                columns: table => new
                {
                    delivery_tracking_history_id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    current_location = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "varchar(50)", nullable: false),
                    notes = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValueSql: "false"),
                    created_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW() AT TIME ZONE 'UTC'"),
                    deleted_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    sorting_center_id = table.Column<long>(type: "bigint", nullable: true),
                    pickup_point_id = table.Column<long>(type: "bigint", nullable: true),
                    delivery_order_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_tracking_history", x => x.delivery_tracking_history_id);
                    table.ForeignKey(
                        name: "FK_delivery_tracking_history_delivery_orders_delivery_order_id",
                        column: x => x.delivery_order_id,
                        principalTable: "delivery_orders",
                        principalColumn: "delivery_order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_delivery_tracking_history_pickup_points_pickup_point_id",
                        column: x => x.pickup_point_id,
                        principalTable: "pickup_points",
                        principalColumn: "pickup_point_id");
                    table.ForeignKey(
                        name: "FK_delivery_tracking_history_sorting_centers_sorting_center_id",
                        column: x => x.sorting_center_id,
                        principalTable: "sorting_centers",
                        principalColumn: "sorting_center_id");
                });

            migrationBuilder.CreateIndex(
                name: "idx_address_application_user_id_btree",
                table: "addresses",
                column: "user_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_address_city_id_btree",
                table: "addresses",
                column: "city_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_address_country_id_btree",
                table: "addresses",
                column: "country_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_address_is_deleted_btree",
                table: "addresses",
                column: "is_deleted")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_cities_country_id",
                table: "cities",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "idx_country_code_btree",
                table: "countries",
                column: "country_code",
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_country_normalized_name_btree",
                table: "countries",
                column: "normalized_country_name",
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_country_normalized_name_ru_btree",
                table: "countries",
                column: "normalized_country_name_ru",
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_order_current_sorting_center_id_btree",
                table: "delivery_orders",
                column: "current_sorting_center_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_order_destination_sorting_center_id_btree",
                table: "delivery_orders",
                column: "destination_sorting_center_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_order_order_id_btree",
                table: "delivery_orders",
                column: "order_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_order_pickup_point_id_btree",
                table: "delivery_orders",
                column: "pickup_point_id")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_orders_delivery_tariff_id",
                table: "delivery_orders",
                column: "delivery_tariff_id");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_tariffs_is_deleted_btree",
                table: "delivery_tariffs",
                column: "is_deleted")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "idx_delivery_tariffs_type_btree",
                table: "delivery_tariffs",
                column: "type")
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_tracking_history_delivery_order_id",
                table: "delivery_tracking_history",
                column: "delivery_order_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_tracking_history_pickup_point_id",
                table: "delivery_tracking_history",
                column: "pickup_point_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_tracking_history_sorting_center_id",
                table: "delivery_tracking_history",
                column: "sorting_center_id");

            migrationBuilder.CreateIndex(
                name: "idx_pickup_point_code_btree",
                table: "pickup_points",
                column: "code",
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_pickup_points_address_id",
                table: "pickup_points",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "idx_sorting_center_code_btree",
                table: "sorting_centers",
                column: "code",
                unique: true)
                .Annotation("Npgsql:IndexMethod", "btree");

            migrationBuilder.CreateIndex(
                name: "IX_sorting_centers_address_id",
                table: "sorting_centers",
                column: "address_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery_tracking_history");

            migrationBuilder.DropTable(
                name: "delivery_orders");

            migrationBuilder.DropTable(
                name: "pickup_points");

            migrationBuilder.DropTable(
                name: "sorting_centers");

            migrationBuilder.DropTable(
                name: "delivery_tariffs");

            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "cities");

            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
