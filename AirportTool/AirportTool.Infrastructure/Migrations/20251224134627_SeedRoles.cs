using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AirportTool.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39ff4620-0033-410c-a066-929e729660b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "75b0b7dc-5756-4027-ba9d-23a1d49c0373");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d", "Staff", "STAFF" },
                    { "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e", "Client", "CLIENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7a8b-9c0d-1e2f3a4b5c6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-6f7a-8b9c-0d1e-2f3a4b5c6d7e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "39ff4620-0033-410c-a066-929e729660b6", null, "Staff", "STAFF" },
                    { "75b0b7dc-5756-4027-ba9d-23a1d49c0373", null, "Client", "CLIENT" }
                });
        }
    }
}
