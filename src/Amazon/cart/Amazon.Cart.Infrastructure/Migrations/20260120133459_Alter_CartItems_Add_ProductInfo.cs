using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Cart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_CartItems_Add_ProductInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImageUrl",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "CartItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImageUrl",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "CartItem");
        }
    }
}
