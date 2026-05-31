using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Cart.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CustomerClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "customer");

            migrationBuilder.CreateTable(
                name: "Claims",
                schema: "customer",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => new { x.CustomerId, x.Key });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claims",
                schema: "customer");
        }
    }
}
