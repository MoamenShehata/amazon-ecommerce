using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Orders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_OutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStore",
                schema: "orders",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "OccurredOn",
                schema: "orders",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "ShouldBePublishedForIntegration",
                schema: "orders",
                table: "EventStore");

            migrationBuilder.RenameTable(
                name: "EventStore",
                schema: "orders",
                newName: "OutboxMessages",
                newSchema: "orders");

            migrationBuilder.RenameColumn(
                name: "HandledAt",
                schema: "orders",
                table: "OutboxMessages",
                newName: "PublishedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "orders",
                table: "OutboxMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "orders",
                table: "OutboxMessages");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "orders",
                newName: "EventStore",
                newSchema: "orders");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                schema: "orders",
                table: "EventStore",
                newName: "HandledAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "OccurredOn",
                schema: "orders",
                table: "EventStore",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ShouldBePublishedForIntegration",
                schema: "orders",
                table: "EventStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStore",
                schema: "orders",
                table: "EventStore",
                column: "Id");
        }
    }
}
