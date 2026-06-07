using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Products_Add_SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "inventory",
                table: "products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "inventory",
                table: "products");
        }
    }
}
