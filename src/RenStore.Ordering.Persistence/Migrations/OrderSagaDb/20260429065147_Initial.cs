using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RenStore.Order.Persistence.Migrations.OrderSagaDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "saga");

            migrationBuilder.CreateTable(
                name: "place_order_sagas",
                schema: "saga",
                columns: table => new
                {
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    current_state = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    size_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price_amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    product_name_snapshot = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    shipping_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    price_received = table.Column<bool>(type: "boolean", nullable: false),
                    address_received = table.Column<bool>(type: "boolean", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: true),
                    failure_reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    timeout_token_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_place_order_sagas", x => x.correlation_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_place_order_sagas_current_state",
                schema: "saga",
                table: "place_order_sagas",
                column: "current_state");

            migrationBuilder.CreateIndex(
                name: "ix_place_order_sagas_customer_id",
                schema: "saga",
                table: "place_order_sagas",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "place_order_sagas",
                schema: "saga");
        }
    }
}
