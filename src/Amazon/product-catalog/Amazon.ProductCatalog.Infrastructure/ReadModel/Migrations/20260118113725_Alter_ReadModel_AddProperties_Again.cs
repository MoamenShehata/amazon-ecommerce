using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.ReadModel.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ReadModel_AddProperties_Again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Categories",
                schema: "catalog.read",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "catalog.read",
                table: "products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                schema: "catalog.read",
                table: "products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                schema: "catalog.read",
                table: "products");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "catalog.read",
                table: "products");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                schema: "catalog.read",
                table: "products");
        }
    }
}
