using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalksAPI.Migrations
{
    /// <inheritdoc />
    public partial class seedingDataforDiffiandRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("185a83b6-eca8-4a40-86bd-44abba6bf337"), "Medium" },
                    { new Guid("278d59ff-285e-4b13-9072-1c5780de16fc"), "Hard" },
                    { new Guid("aa7bc287-a747-4cd4-a652-c337f5d36df8"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("03698f50-8393-4b56-9854-811b16251ead"), "AKL", "Auckland", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("0df0a773-6473-4e7d-8893-50deb5455107"), "WKO", "Waikato", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("77dea13b-c820-4b6e-9086-49aab6b944e1"), "BOP", "Bay of Plenty", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("d428b08e-d4f3-4219-9dc2-3c728446a4be"), "GIS", "Gisborne", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("e5505c6e-6645-4fc9-8f12-d44465e2aaa8"), "NTH", "Northland", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("185a83b6-eca8-4a40-86bd-44abba6bf337"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("278d59ff-285e-4b13-9072-1c5780de16fc"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("aa7bc287-a747-4cd4-a652-c337f5d36df8"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("03698f50-8393-4b56-9854-811b16251ead"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0df0a773-6473-4e7d-8893-50deb5455107"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("77dea13b-c820-4b6e-9086-49aab6b944e1"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("d428b08e-d4f3-4219-9dc2-3c728446a4be"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("e5505c6e-6645-4fc9-8f12-d44465e2aaa8"));
        }
    }
}
