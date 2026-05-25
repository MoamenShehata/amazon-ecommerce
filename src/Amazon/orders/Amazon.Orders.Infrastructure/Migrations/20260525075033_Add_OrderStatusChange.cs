using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_OrderStatusChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "statusHistory",
                schema: "orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    DeliveryMember_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryMember_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrackingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInfo_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInfo_PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInfo_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyInfo_Website = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_statusHistory", x => new { x.OrderId, x.Id });
                    table.ForeignKey(
                        name: "FK_statusHistory_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders",
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statusHistory",
                schema: "orders");
        }
    }
}
