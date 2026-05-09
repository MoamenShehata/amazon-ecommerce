using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_InventoryItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory_InStockCount",
                schema: "inventory",
                table: "products");

            migrationBuilder.CreateTable(
                name: "inventoryItems",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOnHold = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryItems", x => new { x.ProductId, x.Id });
                    table.ForeignKey(
                        name: "FK_inventoryItems_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "inventory",
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventoryItems",
                schema: "inventory");

            migrationBuilder.AddColumn<int>(
                name: "Inventory_InStockCount",
                schema: "inventory",
                table: "products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
