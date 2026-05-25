using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_UseReservedForOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnHold",
                schema: "inventory",
                table: "inventoryItems");

            migrationBuilder.AddColumn<Guid>(
                name: "ReservedForOrder",
                schema: "inventory",
                table: "inventoryItems",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedForOrder",
                schema: "inventory",
                table: "inventoryItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnHold",
                schema: "inventory",
                table: "inventoryItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
