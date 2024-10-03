using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSDGv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SDG_AspNetUsers_UserId",
                table: "SDG");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SDG",
                table: "SDG");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a76f1e3-04cd-41a3-9058-ff28c95dd6f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4eec9535-5bc1-494a-a770-878c47d726cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc9a00dd-d0d8-4569-981d-1bb1bc25327b");

            migrationBuilder.RenameTable(
                name: "SDG",
                newName: "SDGs");

            migrationBuilder.RenameIndex(
                name: "IX_SDG_UserId",
                table: "SDGs",
                newName: "IX_SDGs_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SDGs",
                table: "SDGs",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "023e8d6c-40d1-438d-86c1-dfe05c9c8d6e", "c50fb836-36d3-41c6-ad2c-6a3897f01aba", "User", "USER" },
                    { "03101b58-56da-431d-87dd-7ec1014dd04b", "ab13316b-479e-44d5-bb0c-3e3e19d43ffa", "Human Resources", "Human Resources" },
                    { "69e4cfbb-d193-401b-912d-d9d8d225abf7", "dd1c0c2b-7b9e-411a-8128-2e92f4937b90", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SDGs_AspNetUsers_UserId",
                table: "SDGs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SDGs_AspNetUsers_UserId",
                table: "SDGs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SDGs",
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

            migrationBuilder.RenameTable(
                name: "SDGs",
                newName: "SDG");

            migrationBuilder.RenameIndex(
                name: "IX_SDGs_UserId",
                table: "SDG",
                newName: "IX_SDG_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SDG",
                table: "SDG",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a76f1e3-04cd-41a3-9058-ff28c95dd6f6", "7673d91e-ea44-4d50-ba6e-870443eeaa9d", "Human Resources", "Human Resources" },
                    { "4eec9535-5bc1-494a-a770-878c47d726cb", "c6fb007f-2050-4a0a-9513-d791d3730a18", "User", "USER" },
                    { "cc9a00dd-d0d8-4569-981d-1bb1bc25327b", "f80c903e-6c98-47ca-a9a0-728120f32255", "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_SDG_AspNetUsers_UserId",
                table: "SDG",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
