using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalksAPI.Migrations.Auth
{
    /// <inheritdoc />
    public partial class correctNormalizeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d878464-81c8-481f-92cc-48d3efc8ee0b",
                column: "NormalizedName",
                value: "READER");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5715846-7a07-4fba-bc3e-bfd9dcdd67a9",
                column: "NormalizedName",
                value: "WRITER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d878464-81c8-481f-92cc-48d3efc8ee0b",
                column: "NormalizedName",
                value: "Reader.ToUpper()");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5715846-7a07-4fba-bc3e-bfd9dcdd67a9",
                column: "NormalizedName",
                value: "Writer.ToUpper()");
        }
    }
}
