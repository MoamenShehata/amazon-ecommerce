using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.ReadModel.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ReadModel_AddSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.EnsureSchema(
                name: "read");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products",
                newSchema: "read");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                schema: "read",
                table: "products",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                schema: "read",
                table: "products");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "read",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }
    }
}
