using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.ProductCatalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_OutboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EventStore",
                schema: "catalog",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "OccurredOn",
                schema: "catalog",
                table: "EventStore");

            migrationBuilder.DropColumn(
                name: "ShouldBePublishedForIntegration",
                schema: "catalog",
                table: "EventStore");

            migrationBuilder.RenameTable(
                name: "EventStore",
                schema: "catalog",
                newName: "OutboxMessages",
                newSchema: "catalog");

            migrationBuilder.RenameColumn(
                name: "HandledAt",
                schema: "catalog",
                table: "OutboxMessages",
                newName: "PublishedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "catalog",
                table: "OutboxMessages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OutboxMessages",
                schema: "catalog",
                table: "OutboxMessages");

            migrationBuilder.RenameTable(
                name: "OutboxMessages",
                schema: "catalog",
                newName: "EventStore",
                newSchema: "catalog");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                schema: "catalog",
                table: "EventStore",
                newName: "HandledAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "OccurredOn",
                schema: "catalog",
                table: "EventStore",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "ShouldBePublishedForIntegration",
                schema: "catalog",
                table: "EventStore",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventStore",
                schema: "catalog",
                table: "EventStore",
                column: "Id");
        }
    }
}
