using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.ReadModel.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ReadModel_AddProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog.read");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "read",
                newName: "products",
                newSchema: "catalog.read");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "read");

            migrationBuilder.RenameTable(
                name: "products",
                schema: "catalog.read",
                newName: "products",
                newSchema: "read");
        }
    }
}
