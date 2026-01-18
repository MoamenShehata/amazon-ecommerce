using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Categories_AddFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                schema: "catalog",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                schema: "catalog",
                table: "Categories");
        }
    }
}
