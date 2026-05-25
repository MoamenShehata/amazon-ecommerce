using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_OrderStatusChange_Remove_Id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_statusHistory",
                schema: "orders",
                table: "statusHistory");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "orders",
                table: "statusHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_statusHistory",
                schema: "orders",
                table: "statusHistory",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_statusHistory",
                schema: "orders",
                table: "statusHistory");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "orders",
                table: "statusHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_statusHistory",
                schema: "orders",
                table: "statusHistory",
                columns: new[] { "OrderId", "Id" });
        }
    }
}
