using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Customers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_PaymentCards_SetAsOwnedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards");

            migrationBuilder.DropIndex(
                name: "IX_PaymentCards_CustomerId",
                table: "PaymentCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards",
                columns: new[] { "CustomerId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCards_CustomerId",
                table: "PaymentCards",
                column: "CustomerId");
        }
    }
}
