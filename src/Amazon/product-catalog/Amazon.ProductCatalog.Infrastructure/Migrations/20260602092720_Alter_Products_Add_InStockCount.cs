using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Products_Add_InStockCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InStockCount",
                schema: "catalog",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InStockCount",
                schema: "catalog",
                table: "Products");
        }
    }
}
