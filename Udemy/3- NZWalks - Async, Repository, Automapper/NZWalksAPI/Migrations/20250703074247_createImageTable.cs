using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalksAPI.Migrations
{
    /// <inheritdoc />
    public partial class createImageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("27a4b336-6632-47b3-8ee5-ceb21299f555"), "Hard" },
                    { new Guid("e81831a8-d2e1-4b47-bfda-b378227ea681"), "Easy" },
                    { new Guid("edccd29e-e9a9-490c-8ef7-95639a3438d3"), "Medium" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("4abced85-4245-43e3-90aa-795dc043d4d8"), "BOP", "Bay of Plenty", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("8d03f678-e807-4e4c-9c9a-9e84fd3afa41"), "AKL", "Auckland", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("c4b0e728-2dd1-4b39-8e68-e1c973f02c30"), "NTH", "Northland", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("e45a0f11-a18b-45a5-a09f-5af2d4e828a7"), "WKO", "Waikato", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" },
                    { new Guid("f392bebf-4fea-405c-b45b-70f9a5e9ecf7"), "GIS", "Gisborne", "https://images.pexels.com/photos/2325447/pexels-photo-2325447.jpeg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("27a4b336-6632-47b3-8ee5-ceb21299f555"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("e81831a8-d2e1-4b47-bfda-b378227ea681"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("edccd29e-e9a9-490c-8ef7-95639a3438d3"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("4abced85-4245-43e3-90aa-795dc043d4d8"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("8d03f678-e807-4e4c-9c9a-9e84fd3afa41"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("c4b0e728-2dd1-4b39-8e68-e1c973f02c30"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("e45a0f11-a18b-45a5-a09f-5af2d4e828a7"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f392bebf-4fea-405c-b45b-70f9a5e9ecf7"));

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
    }
}
