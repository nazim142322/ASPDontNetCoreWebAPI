using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalksAPI.Migrations.Auth
{
    /// <inheritdoc />
    public partial class seedingRoleAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "24b862e9-bb00-4c1a-9c50-5765924c2176", "24b862e9-bb00-4c1a-9c50-5765924c2176", "Dummy", "DUMMY" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24b862e9-bb00-4c1a-9c50-5765924c2176");
        }
    }
}
