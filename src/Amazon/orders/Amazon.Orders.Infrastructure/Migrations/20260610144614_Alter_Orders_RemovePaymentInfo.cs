using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Orders_RemovePaymentInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentInfo",
                schema: "orders",
                table: "orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentInfo",
                schema: "orders",
                table: "orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
