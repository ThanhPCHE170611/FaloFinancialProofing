using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSDGv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SDGs_UserId",
                table: "SDGs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "023e8d6c-40d1-438d-86c1-dfe05c9c8d6e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03101b58-56da-431d-87dd-7ec1014dd04b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69e4cfbb-d193-401b-912d-d9d8d225abf7");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "056f8aca-62f1-4431-bbff-453c2cf01a6d", "2c55592f-24a7-47d0-95ca-6c3c4b67c8fa", "User", "USER" },
                    { "21d7ce15-d438-4ec7-b20d-f59655afe7b7", "0544c988-b90b-4bee-948a-7117b26ab303", "Admin", "ADMIN" },
                    { "61370db3-90e6-4a4e-a3f1-f9a745f82087", "f7686586-9ed9-4007-82b5-9744a6b70a12", "Human Resources", "Human Resources" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SDGs_UserId",
                table: "SDGs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SDGs_UserId",
                table: "SDGs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "056f8aca-62f1-4431-bbff-453c2cf01a6d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21d7ce15-d438-4ec7-b20d-f59655afe7b7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61370db3-90e6-4a4e-a3f1-f9a745f82087");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "023e8d6c-40d1-438d-86c1-dfe05c9c8d6e", "c50fb836-36d3-41c6-ad2c-6a3897f01aba", "User", "USER" },
                    { "03101b58-56da-431d-87dd-7ec1014dd04b", "ab13316b-479e-44d5-bb0c-3e3e19d43ffa", "Human Resources", "Human Resources" },
                    { "69e4cfbb-d193-401b-912d-d9d8d225abf7", "dd1c0c2b-7b9e-411a-8128-2e92f4937b90", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SDGs_UserId",
                table: "SDGs",
                column: "UserId",
                unique: true);
        }
    }
}
