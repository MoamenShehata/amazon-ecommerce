using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_OutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStore",
                schema: "inventory",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "OccurredOn",
                schema: "inventory",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "ShouldBePublishedForIntegration",
                schema: "inventory",
                table: "EventStore");

            migrationBuilder.RenameTable(
                name: "EventStore",
                schema: "inventory",
                newName: "OutboxMessages",
                newSchema: "inventory");

            migrationBuilder.RenameColumn(
                name: "HandledAt",
                schema: "inventory",
                table: "OutboxMessages",
                newName: "PublishedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "inventory",
                table: "OutboxMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "inventory",
                table: "OutboxMessages");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "inventory",
                newName: "EventStore",
                newSchema: "inventory");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                schema: "inventory",
                table: "EventStore",
                newName: "HandledAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "OccurredOn",
                schema: "inventory",
                table: "EventStore",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ShouldBePublishedForIntegration",
                schema: "inventory",
                table: "EventStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStore",
                schema: "inventory",
                table: "EventStore",
                column: "Id");
        }
    }
}
