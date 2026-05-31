using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ShipingAddress_AddPhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "House_PhoneNumber",
                table: "ShippingAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "House_PhoneNumber",
                table: "ShippingAddress");
        }
    }
}
