using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Customers.Infrastructure.Migrations.CustomerRead
{
    /// <inheritdoc />
    public partial class Alter_Profiles_AddPaymentCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentCards",
                schema: "read",
                table: "customerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentCards",
                schema: "read",
                table: "customerProfiles");
        }
    }
}
