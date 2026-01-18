using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazon.Identity.Presentation.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Roles_Seed_Admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "326c7bba-a7dc-4ff1-bcba-39d09711fa95", null, "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "326c7bba-a7dc-4ff1-bcba-39d09711fa95");
        }
    }
}
