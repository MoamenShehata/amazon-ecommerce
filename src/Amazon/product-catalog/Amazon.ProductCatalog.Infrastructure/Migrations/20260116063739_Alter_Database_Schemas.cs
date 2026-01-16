using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Database_Schemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "catalog");

            migrationBuilder.RenameTable(
                name: "EventStore",
                newName: "EventStore",
                newSchema: "catalog");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "catalog");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Products",
                schema: "catalog",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "EventStore",
                schema: "catalog",
                newName: "EventStore");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "catalog",
                newName: "Categories");
        }
    }
}
