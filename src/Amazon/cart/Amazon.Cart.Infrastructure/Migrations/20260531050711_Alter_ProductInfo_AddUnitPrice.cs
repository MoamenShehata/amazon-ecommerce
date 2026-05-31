using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Cart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ProductInfo_AddUnitPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Info_UnitPrice",
                table: "Product",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Info_UnitPrice",
                table: "Product");
        }
    }
}
