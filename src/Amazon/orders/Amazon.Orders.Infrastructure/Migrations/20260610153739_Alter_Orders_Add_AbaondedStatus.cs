using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Orders_Add_AbaondedStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                schema: "orders",
                table: "statusHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                schema: "orders",
                table: "statusHistory");
        }
    }
}
