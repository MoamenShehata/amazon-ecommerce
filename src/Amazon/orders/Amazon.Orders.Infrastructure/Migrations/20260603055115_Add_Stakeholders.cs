using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Stakeholders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Customer_Id",
                schema: "orders",
                table: "orders",
                newName: "Owner_Id");

            migrationBuilder.RenameColumn(
                name: "Customer_Email",
                schema: "orders",
                table: "orders",
                newName: "Owner_Email");

            migrationBuilder.CreateTable(
                name: "StakeHolder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakeHolder", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StakeHolder");

            migrationBuilder.RenameColumn(
                name: "Owner_Id",
                schema: "orders",
                table: "orders",
                newName: "Customer_Id");

            migrationBuilder.RenameColumn(
                name: "Owner_Email",
                schema: "orders",
                table: "orders",
                newName: "Customer_Email");
        }
    }
}
